using AutoMapper;
using Castle.Core.Internal;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Business.Models.UserModels;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelReservation.Business.Services
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<UserEntity> _passwordHasher;
        private readonly IHotelRepository _hotelRepo;
        private readonly ILogger _logger;

        public UsersService(
            UserManager<UserEntity> userManager,
            IPasswordHasher<UserEntity> passwordHasher,
            IHotelRepository hotelRepo,
            IMapper mapper,
            ILogger logger)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _hotelRepo = hotelRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
        {
            _logger.Debug("Users are requesting");

            var userModels = _mapper.Map<IEnumerable<UserModel>>(_userManager.Users);

            var users = userModels.ToList();
            foreach (var userModel in users)
            {
                await GetRolesForUserModelAsync(userModel);
            }

            _logger.Debug("Users requested");

            return users;
        }

        public async Task<UserModel> CreateAsync(UserRegistrationModel userRegistration)
        {
            _logger.Debug($"User {userRegistration?.Email} is creating");

            if (userRegistration == null)
                throw new BusinessException("User cannot be empty", ErrorStatus.EmptyInput);

            var existingUserEntity = await _userManager.FindByEmailAsync(userRegistration.Email);

            if (existingUserEntity != null)
                throw new BusinessException("User with such email already exists", ErrorStatus.AlreadyExist);

            var userEntity = _mapper.Map<UserEntity>(userRegistration);

            userEntity.UserName ??= userRegistration.Email.Split('@', StringSplitOptions.RemoveEmptyEntries)[0];
            userEntity.PasswordHash = _passwordHasher.HashPassword(userEntity, userRegistration.Password);

            var result = await _userManager.CreateAsync(userEntity);
            var addedUserEntity = await _userManager.FindByEmailAsync(userRegistration.Email);
            var addedUserRoles = await _userManager.GetRolesAsync(addedUserEntity);

            if (result == IdentityResult.Success && addedUserRoles.IsNullOrEmpty())
            {
                await _userManager.AddToRoleAsync(addedUserEntity, "User");
                foreach (var role in userRegistration.Roles)
                {
                    if (role.ToUpper() == "ADMIN" || role.ToUpper() == "MANAGER")
                    {
                        try
                        {
                            await _userManager.AddToRoleAsync(addedUserEntity, role);
                        }
                        catch
                        {
                            throw new BusinessException(
                                $"Cannot add user to unknown role {role}",
                                ErrorStatus.IncorrectInput);
                        }
                    }
                }
            }

            var addedUserModel = _mapper.Map<UserModel>(addedUserEntity);
            await GetRolesForUserModelAsync(addedUserModel);

            _logger.Debug($"Users {userRegistration.Email} created");

            return addedUserModel;
        }

        public async Task<UserModel> GetAsync(string id)
        {
            _logger.Debug($"User {id} is requesting");

            var userEntity = await _userManager.FindByIdAsync(id);
            var userModel = _mapper.Map<UserModel>(userEntity);
            await GetRolesForUserModelAsync(userModel);

            _logger.Debug($"User {id} requested");

            return userModel;
        }

        public async Task<UserModel> DeleteAsync(string id, IEnumerable<Claim> currentUserClaims)
        {
            _logger.Debug($"User {id} is deleting");

            if (currentUserClaims.FirstOrDefault(cl => cl.Type == "id")?.Value == id)
                throw new BusinessException("You cannot delete yourself", ErrorStatus.IncorrectInput);

            var deletedUserEntity = await _userManager.FindByIdAsync(id);

            var deletedUserModel = _mapper.Map<UserModel>(deletedUserEntity);
            await GetRolesForUserModelAsync(deletedUserModel);

            var result = await _userManager.DeleteAsync(deletedUserEntity);

            if (result != IdentityResult.Success)
                throw new BusinessException("No user with such id", ErrorStatus.NotFound, result.Errors);

            _logger.Debug($"User {id} deleted");

            return deletedUserModel;
        }

        public async Task<UserModel> UpdateAsync(string id, UserUpdateModel updatingUserUpdateModel, IEnumerable<Claim> currentUserClaims)
        {
            _logger.Debug($"User {id} is updating");

            if (updatingUserUpdateModel == null)
                throw new BusinessException("User cannot be empty", ErrorStatus.EmptyInput);

            var userEntity = await _userManager.FindByIdAsync(id);

            if (userEntity == null)
                throw new BusinessException("User with such id does not exist", ErrorStatus.NotFound);

            if (updatingUserUpdateModel.Email != null)
                userEntity.Email = updatingUserUpdateModel.Email;

            if (updatingUserUpdateModel.PhoneNumber != null)
                userEntity.PhoneNumber = updatingUserUpdateModel.PhoneNumber;

            if (updatingUserUpdateModel.DateOfBirth != null)
                userEntity.DateOfBirth = updatingUserUpdateModel.DateOfBirth.Value;

            if (updatingUserUpdateModel.FirstName != null)
                userEntity.FirstName = updatingUserUpdateModel.FirstName;

            if (updatingUserUpdateModel.LastName != null)
                userEntity.LastName = updatingUserUpdateModel.LastName;

            if (updatingUserUpdateModel.UserName != null)
                userEntity.UserName = updatingUserUpdateModel.UserName;

            if (updatingUserUpdateModel.HotelId != null)
            {
                var unused = _hotelRepo.GetAsync(updatingUserUpdateModel.HotelId.Value, true) ??
                                  throw new BusinessException("There is no hotel with such id", ErrorStatus.NotFound);

                userEntity.HotelId = updatingUserUpdateModel.HotelId;
            }

            if (updatingUserUpdateModel.Email != null)
                userEntity.UserName ??= updatingUserUpdateModel.Email.Split('@', StringSplitOptions.RemoveEmptyEntries)[0];

            if (updatingUserUpdateModel.NewPassword != null)
            {
                var changePasswordResult = await _userManager.ChangePasswordAsync(
                    userEntity,
                    updatingUserUpdateModel.OldPassword,
                    updatingUserUpdateModel.NewPassword);

                if (changePasswordResult != IdentityResult.Success)
                    throw new BusinessException("Incorrect previous password", ErrorStatus.IncorrectInput, changePasswordResult.Errors);
            }

            var result = await _userManager.UpdateAsync(userEntity);

            var updatedUserEntity = await _userManager.FindByEmailAsync(userEntity.Email);

            if (result == IdentityResult.Success)
            {
                if (updatingUserUpdateModel.Roles != null)
                {
                    var addedUserRoles = await _userManager.GetRolesAsync(updatedUserEntity);

                    if (!addedUserRoles.Contains("User"))
                        await _userManager.AddToRoleAsync(updatedUserEntity, "User");

                    for (var i = 0; i < updatingUserUpdateModel.Roles.Count; ++i)
                    {
                        updatingUserUpdateModel.Roles[i] = updatingUserUpdateModel.Roles[i].ToUpper();
                    }

                    foreach (var role in addedUserRoles)
                    {
                        if (!updatingUserUpdateModel.Roles.Contains(role.ToUpper()))
                        {
                            if (role.ToUpper() == "ADMIN" &&
                                updatedUserEntity.Id.Equals(currentUserClaims.FirstOrDefault(cl => cl.Type == "id").Value))
                            {
                                throw new BusinessException(
                                    "You cannot change your own admin role",
                                    ErrorStatus.IncorrectInput);
                            }

                            if (role.ToUpper() == "USER")
                            {
                                continue;
                            }

                            await _userManager.RemoveFromRoleAsync(updatedUserEntity, role.ToUpper());
                        }
                    }

                    foreach (var role in updatingUserUpdateModel.Roles)
                    {
                        try
                        {
                            await _userManager.AddToRoleAsync(updatedUserEntity, role);
                        }
                        catch
                        {
                            throw new BusinessException(
                                $"Unable to add user to role {role}",
                                ErrorStatus.IncorrectInput);
                        }
                    }
                }
            }

            var addedUserModel = _mapper.Map<UserModel>(updatedUserEntity);
            await GetRolesForUserModelAsync(addedUserModel);

            _logger.Debug($"User {id} updated");

            return addedUserModel;
        }

        private async Task GetRolesForUserModelAsync(UserModel userModel)
        {
            _logger.Debug($"User {userModel.Id} roles are requesting");

            var roles = await _userManager.GetRolesAsync(_mapper.Map<UserEntity>(userModel));
            userModel.Roles = roles;

            _logger.Debug($"User {userModel.Id} roles requested");
        }
    }
}
