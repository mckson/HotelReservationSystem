using AutoMapper;
using Castle.Core.Internal;
using HotelReservation.API.Application.Commands.User;
using HotelReservation.API.Application.Interfaces;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.User
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UserResponseModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IMapper _mapper;
        private readonly IAuthenticationHelper _authenticationHelper;

        public UpdateUserHandler(
            IUserRepository userRepository,
            IMapper mapper,
            IHotelRepository hotelRepository,
            UserManager<UserEntity> userManager,
            IAuthenticationHelper authenticationHelper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _hotelRepository = hotelRepository;
            _userManager = userManager;
            _authenticationHelper = authenticationHelper;
        }

        public async Task<UserResponseModel> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new BusinessException("User cannot be empty", ErrorStatus.EmptyInput);

            var accessAllowed = _authenticationHelper.CheckGetUserPermission(request.Id);

            if (!accessAllowed)
            {
                throw new BusinessException(
                    "You cannot access data about that user. You can access data only about yourself",
                    ErrorStatus.AccessDenied);
            }

            var userEntity = await _userRepository.GetByIdAsync(request.Id);

            if (request.Email != null)
            {
                var existentUserEntity = await _userRepository.GetByEmailAsync(request.Email);

                if (existentUserEntity == null || existentUserEntity.Id.Equals(request.Id))
                {
                    userEntity.Email = request.Email;
                }
                else
                {
                    throw new BusinessException(
                        $"User with email {request.Email} already exists",
                        ErrorStatus.AlreadyExist);
                }
            }

            if (request.PhoneNumber != null)
                userEntity.PhoneNumber = request.PhoneNumber;

            userEntity.DateOfBirth = request.DateOfBirth;

            if (request.FirstName != null)
                userEntity.FirstName = request.FirstName;

            if (request.LastName != null)
                userEntity.LastName = request.LastName;

            if (request.UserName != null)
            {
                var existentUserEntity = await _userRepository.GetByNameAsync(request.UserName);

                if (existentUserEntity == null || existentUserEntity.Id.Equals(request.Id))
                {
                    userEntity.UserName = request.UserName;
                }
                else
                {
                    throw new BusinessException(
                        $"User with name {request.UserName} already exists",
                        ErrorStatus.AlreadyExist);
                }
            }

            var hotelUsers = new List<HotelUserEntity>();
            userEntity.HotelUsers.RemoveAll(hu => hu.UserId == userEntity.Id);

            if (request.Hotels != null)
            {
                foreach (var hotel in request.Hotels)
                {
                    var unused = await _hotelRepository.GetAsync(hotel) ??
                                 throw new BusinessException("There is no hotel with such id", ErrorStatus.NotFound);

                    hotelUsers.Add(new HotelUserEntity() { HotelId = hotel });
                }
            }

            if (request.Email != null)
                userEntity.UserName ??= request.Email.Split('@', StringSplitOptions.RemoveEmptyEntries)[0];

            if (!request.NewPassword.IsNullOrEmpty())
            {
                var changePasswordResult = await _userManager.ChangePasswordAsync(
                    userEntity,
                    request.OldPassword,
                    request.NewPassword);

                if (changePasswordResult != IdentityResult.Success)
                {
                    var sb = new StringBuilder();
                    foreach (var error in changePasswordResult.Errors)
                    {
                        sb.Append(error.Description + ' ');
                    }

                    throw new BusinessException(sb.ToString(), ErrorStatus.IncorrectInput, changePasswordResult.Errors);
                }
            }

            if (hotelUsers.Count > 0)
            {
                userEntity.HotelUsers = hotelUsers;
            }

            var result = await _userRepository.UpdateAsync(userEntity);

            if (result)
            {
                if (request.Roles != null)
                {
                    var addedUserRoles = userEntity.Roles;
                    // var userId = Guid.Parse(currentUserClaims.FirstOrDefault(cl => cl.Type == ClaimNames.Id).Value);
                    if (!addedUserRoles.Contains(Roles.User))
                        await _userRepository.AddToRoleAsync(userEntity, Roles.User);

                    foreach (var role in addedUserRoles)
                    {
                        if (!request.Roles.Contains(role, StringComparer.InvariantCultureIgnoreCase))
                        {
                            // if (string.Equals(role, Roles.Admin, StringComparison.InvariantCultureIgnoreCase) &&
                            //     userEntity.Id.Equals(userId))
                            // {
                            //     throw new BusinessException(
                            //         "You cannot change your own admin role",
                            //         ErrorStatus.IncorrectInput);
                            // }
                            if (string.Equals(role, Roles.User, StringComparison.InvariantCultureIgnoreCase))
                            {
                                continue;
                            }

                            await _userManager.RemoveFromRoleAsync(userEntity, role.ToUpper());
                        }
                    }

                    foreach (var role in request.Roles)
                    {
                        if (!addedUserRoles.Contains(role, StringComparer.InvariantCultureIgnoreCase))
                        {
                            var roleResult = await _userRepository.AddToRoleAsync(userEntity, role);
                            if (!roleResult)
                            {
                                throw new BusinessException(
                                    $"Cannot add user to role {role}, because it is already in role or it is nonexistent role",
                                    ErrorStatus.IncorrectInput);
                            }
                        }
                    }
                }
            }

            var updatedUserEntity = await _userRepository.GetByIdAsync(userEntity.Id);
            var addedUserResponse = _mapper.Map<UserResponseModel>(updatedUserEntity);

            return addedUserResponse;
        }
    }
}
