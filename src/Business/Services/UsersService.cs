using AutoMapper;
using Castle.Core.Internal;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Business.Models.UserModels;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
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

        public UsersService(
            UserManager<UserEntity> userManager,
            IPasswordHasher<UserEntity> passwordHasher,
            IHotelRepository hotelRepo,
            IMapper mapper)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _hotelRepo = hotelRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
        {
            var userModels = _mapper.Map<IEnumerable<UserModel>>(_userManager.Users);
            foreach (var userModel in userModels)
            {
                await GetRolesForUserModelAsync(userModel);
            }

            return userModels;
        }

        public async Task<UserModel> CreateAsync(UserRegistrationModel userRegistration)
        {
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
                        await _userManager.AddToRoleAsync(addedUserEntity, role);
                }
            }

            var addedUserModel = _mapper.Map<UserModel>(addedUserEntity);
            await GetRolesForUserModelAsync(addedUserModel);

            return addedUserModel;
        }

        public async Task<UserModel> GetAsync(string id)
        {
            var userEntity = await _userManager.FindByIdAsync(id);
            var userModel = _mapper.Map<UserModel>(userEntity);
            await GetRolesForUserModelAsync(userModel);
            return userModel;
        }

        public async Task<UserModel> DeleteAsync(string id, IEnumerable<Claim> currentUserClaims)
        {
            // could be removed
            if (currentUserClaims.FirstOrDefault(cl => cl.Type == "id").Value == id)
                throw new BusinessException("You cannot delete yourself", ErrorStatus.IncorrectInput);

            var deletedUserEntity = await _userManager.FindByIdAsync(id);

            var deletedUserModel = _mapper.Map<UserModel>(deletedUserEntity);
            await GetRolesForUserModelAsync(deletedUserModel);

            var result = await _userManager.DeleteAsync(deletedUserEntity);

            if (result != IdentityResult.Success)
                throw new BusinessException("No user with such id", ErrorStatus.NotFound);

            return deletedUserModel;
        }

        public async Task<UserModel> UpdateAsync(string id, UserUpdateModel userUpdateModel, IEnumerable<Claim> currentUserClaims)
        {
            if (userUpdateModel == null)
                throw new BusinessException("User cannot be empty", ErrorStatus.EmptyInput);

            var userEntity = await _userManager.FindByIdAsync(id);

            if (userEntity == null)
                throw new BusinessException("User with such id does not exist", ErrorStatus.NotFound);

            if (userUpdateModel.Email != null)
                userEntity.Email = userUpdateModel.Email;

            if (userUpdateModel.PhoneNumber != null)
                userEntity.PhoneNumber = userUpdateModel.PhoneNumber;

            if (userUpdateModel.DateOfBirth != null)
                userEntity.DateOfBirth = userUpdateModel.DateOfBirth.Value;

            if (userUpdateModel.FirstName != null)
                userEntity.FirstName = userUpdateModel.FirstName;

            if (userUpdateModel.LastName != null)
                userEntity.LastName = userUpdateModel.LastName;

            if (userUpdateModel.UserName != null)
                userEntity.UserName = userUpdateModel.UserName;

            if (userUpdateModel.HotelId != null)
            {
                var hotelEntity = _hotelRepo.GetAsync(userUpdateModel.HotelId.Value, true) ??
                                  throw new BusinessException("There is no hotel with such id", ErrorStatus.NotFound);
                userEntity.HotelId = userUpdateModel.HotelId;
            }

            userEntity.UserName ??= userUpdateModel.Email.Split('@', StringSplitOptions.RemoveEmptyEntries)[0];

            if (userUpdateModel.NewPassword != null)
            {
                var changePasswordResult = await _userManager.ChangePasswordAsync(
                    userEntity,
                    userUpdateModel.OldPassword,
                    userUpdateModel.NewPassword);

                if (changePasswordResult != IdentityResult.Success)
                    throw new BusinessException("Incorrect previous password", ErrorStatus.IncorrectInput);
            }

            var result = await _userManager.UpdateAsync(userEntity);

            var updatedUserEntity = await _userManager.FindByEmailAsync(userEntity.Email);

            if (result == IdentityResult.Success)
            {
                if (userUpdateModel.Roles != null)
                {
                    var addedUserRoles = await _userManager.GetRolesAsync(updatedUserEntity);

                    if (!addedUserRoles.Contains("User"))
                        await _userManager.AddToRoleAsync(updatedUserEntity, "User");

                    for (int i = 0; i < userUpdateModel.Roles.Count(); ++i)
                    {
                        userUpdateModel.Roles[i] = userUpdateModel.Roles[i].ToUpper();
                    }

                    foreach (var role in addedUserRoles)
                    {
                        if (!userUpdateModel.Roles.Contains(role.ToUpper()))
                        {
                            if (role.ToUpper() == "ADMIN" &&
                                updatedUserEntity.Id == currentUserClaims.FirstOrDefault(cl => cl.Type == "id").Value)
                            {
                                throw new BusinessException(
                                    "You cannot change your own admin role",
                                    ErrorStatus.IncorrectInput);
                            }

                            await _userManager.RemoveFromRoleAsync(updatedUserEntity, role.ToUpper());
                        }
                    }

                    foreach (var role in userUpdateModel.Roles)
                    {
                        // if (role.ToUpper() == "ADMIN" || role.ToUpper() == "MANAGER")
                            await _userManager.AddToRoleAsync(updatedUserEntity, role);
                    }
                }
            }

            var addedUserModel = _mapper.Map<UserModel>(updatedUserEntity);
            await GetRolesForUserModelAsync(addedUserModel);

            return addedUserModel;
        }

        private async Task GetRolesForUserModelAsync(UserModel userModel)
        {
            var roles = await _userManager.GetRolesAsync(_mapper.Map<UserEntity>(userModel));
            userModel.Roles = roles;
        }
    }
}
