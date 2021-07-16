using AutoMapper;
using HotelReservation.API.Application.Commands.User;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.User
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserResponseModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<UserEntity> _passwordHasher;
        private readonly IMapper _mapper;

        public CreateUserHandler(
            IUserRepository userRepository,
            IMapper mapper,
            IPasswordHasher<UserEntity> passwordHasher)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserResponseModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new BusinessException("User cannot be empty", ErrorStatus.EmptyInput);

            var existingUserEntity = await _userRepository.GetByEmailAsync(request.Email);

            if (existingUserEntity != null)
                throw new BusinessException("User with such email already exists", ErrorStatus.AlreadyExist);

            var userEntity = _mapper.Map<UserEntity>(request);

            userEntity.UserName ??= request.Email.Split('@', StringSplitOptions.RemoveEmptyEntries)[0];
            userEntity.PasswordHash = _passwordHasher.HashPassword(userEntity, request.Password);

            if (request.Hotels != null && request.Hotels.Any())
            {
                userEntity.HotelUsers = new List<HotelUserEntity>();
                userEntity.HotelUsers.AddRange(request.Hotels.Select(hotelId => new HotelUserEntity
                {
                    HotelId = hotelId
                }));
            }

            var result = await _userRepository.CreateAsync(userEntity);
            if (result)
            {
                foreach (var role in request.Roles)
                {
                    var roleResult = await _userRepository.AddToRoleAsync(userEntity, role);

                    if (!roleResult)
                    {
                        await _userRepository.DeleteAsync(userEntity.Id);
                        throw new BusinessException(
                            $"Cannot add user to unknown role {role}",
                            ErrorStatus.IncorrectInput);
                    }
                }
            }
            else
            {
                throw new BusinessException(
                    "User with such nickname or email already exists",
                    ErrorStatus.IncorrectInput);
            }

            var addedUserEntity = await _userRepository.GetByIdAsync(userEntity.Id); // to attract roles on repository level
            var addedUserResponse = _mapper.Map<UserResponseModel>(addedUserEntity);

            return addedUserResponse;
        }
    }
}
