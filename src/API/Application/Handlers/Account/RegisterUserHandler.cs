using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Castle.Core.Internal;
using HotelReservation.API.Application.Commands.Account;
using HotelReservation.Business;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace HotelReservation.API.Application.Handlers.Account
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<UserEntity> _passwordHasher;

        public RegisterUserHandler(IUserRepository userRepository, ILogger logger, IMapper mapper, IPasswordHasher<UserEntity> passwordHasher)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            _logger.Debug($"User {request.Email} is signing up");

            if (request == null)
                throw new BusinessException("User cannot be empty", ErrorStatus.EmptyInput);

            var existingUserEntity = await _userRepository.GetByEmailAsync(request.Email);

            if (existingUserEntity != null)
                throw new BusinessException("User with such email already exists", ErrorStatus.AlreadyExist);

            var userEntity = _mapper.Map<UserEntity>(request);

            userEntity.UserName ??= request.Email.Split('@', StringSplitOptions.RemoveEmptyEntries)[0];
            userEntity.PasswordHash = _passwordHasher.HashPassword(userEntity, request.Password);

            var result = await _userRepository.CreateAsync(userEntity);

            if (result)
            {
                var addedUserEntity = await _userRepository.GetByEmailAsync(request.Email);
                var addedUserRoles = addedUserEntity.Roles;

                if (addedUserRoles.IsNullOrEmpty())
                {
                    await _userRepository.AddToRoleAsync(addedUserEntity, "User");
                }
            }
            else
            {
                throw new BusinessException("Unable to register user with such parameters", ErrorStatus.AlreadyExist);
            }

            _logger.Debug($"User {request.Email} signed up");

            return Unit.Value;
        }
    }
}
