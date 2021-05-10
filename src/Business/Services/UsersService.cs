using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Castle.Core.Internal;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models.UserModels;
using HotelReservation.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace HotelReservation.Business.Services
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPasswordHasher<UserEntity> _passwordHasher;

        public UsersService(
            UserManager<UserEntity> userManager,
            RoleManager<IdentityRole> roleManger,
            IPasswordHasher<UserEntity> passwordHasher,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManger;
            _passwordHasher = passwordHasher;
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
                throw new DataException("User cannot be empty", ErrorStatus.EmptyInput);

            var existingUserEntity = await _userManager.FindByEmailAsync(userRegistration.Email);

            if (existingUserEntity != null)
                throw new DataException("User with such email already exists", ErrorStatus.AlreadyExist);

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

        public async Task<UserModel> DeleteAsync(string id)
        {
            var deletedUserEntity = await _userManager.FindByIdAsync(id);
            var deletedUserModel = _mapper.Map<UserModel>(deletedUserEntity);
            await GetRolesForUserModelAsync(deletedUserModel);

            var result = await _userManager.DeleteAsync(deletedUserEntity);

            return deletedUserModel;
        }

        public async Task<UserModel> UpdateAsync(string id, UserUpdateModel userUpdateModel)
        {
            if (userUpdateModel == null)
                throw new DataException("User cannot be empty", ErrorStatus.EmptyInput);

            var userEntity = await _userManager.FindByIdAsync(id);

            if (userEntity == null)
                throw new DataException("User with such id does not exist", ErrorStatus.NotFound);

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

            userEntity.UserName ??= userUpdateModel.Email.Split('@', StringSplitOptions.RemoveEmptyEntries)[0];

            if (userUpdateModel.NewPassword != null)
            {
                var changePasswordResult = await _userManager.ChangePasswordAsync(
                    userEntity,
                    userUpdateModel.OldPassword,
                    userUpdateModel.NewPassword);

                if (changePasswordResult != IdentityResult.Success)
                    throw new DataException("Incorrect previous password", ErrorStatus.IncorrectInput);
            }

            var result = await _userManager.UpdateAsync(userEntity);

            var updatedUserEntity = await _userManager.FindByEmailAsync(userEntity.Email);

            var addedUserRoles = await _userManager.GetRolesAsync(updatedUserEntity);

            if (result == IdentityResult.Success)
            {
                if (!addedUserRoles.Contains("User"))
                    await _userManager.AddToRoleAsync(updatedUserEntity, "User");

                for (int i = 0; i < userUpdateModel.Roles.Count(); ++i)
                {
                    userUpdateModel.Roles[i] = userUpdateModel.Roles[i].ToUpper();
                }

                foreach (var role in addedUserRoles)
                {
                    if (!userUpdateModel.Roles.Contains(role.ToUpper()))
                        await _userManager.RemoveFromRoleAsync(updatedUserEntity, role.ToUpper());
                }

                foreach (var role in userUpdateModel.Roles)
                {
                    if (role.ToUpper() == "ADMIN" || role.ToUpper() == "MANAGER")
                        await _userManager.AddToRoleAsync(updatedUserEntity, role);
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
