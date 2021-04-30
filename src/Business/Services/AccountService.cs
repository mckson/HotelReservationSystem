using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Castle.Core.Internal;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models.RequestModels;
using HotelReservation.Business.Models.ResponseModels;
using HotelReservation.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace HotelReservation.Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IPasswordHasher<UserEntity> _passwordHasher;

        public AccountService(UserManager<UserEntity> userManager, 
            IPasswordHasher<UserEntity> passwordHasher)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserResponseModel> AuthenticateAsync(string email, string password)
        {
            if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password))
                return null;

            var userEntity = await _userManager.FindByEmailAsync(email);
            if (userEntity == null)
            {
                return null;
            }
            //if (userEntity.PasswordHash != password)
            //{
            //    return null;
            //}

            if (_passwordHasher.VerifyHashedPassword(userEntity, userEntity.PasswordHash, password) ==
                PasswordVerificationResult.Failed)
                return null;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserEntity, UserResponseModel>()
                .ForMember("Password", opt => opt.MapFrom(c => c.PasswordHash)));
            var mapper = new Mapper(config);

            return mapper.Map<UserEntity, UserResponseModel>(userEntity);

        }

        public async Task<UserResponseModel> RegisterAsync(UserRegistrationRequestModel user, string password)
        {
            if (String.IsNullOrEmpty(password) || user == null)
                return null;

            var existingUserEntity = await _userManager.FindByEmailAsync(user.Email);

            if (existingUserEntity != null)
                return null;    //add error "user with such email exists"

            user.Password = password;

            var configEntity = new MapperConfiguration(cfg => cfg.CreateMap<UserRegistrationRequestModel, UserEntity>());
            var mapperEntity = new Mapper(configEntity);

            var userEntity = mapperEntity.Map<UserRegistrationRequestModel, UserEntity>(user);
            userEntity.UserName ??= user.Email.Split('@', StringSplitOptions.RemoveEmptyEntries)[0];
            userEntity.PasswordHash = _passwordHasher.HashPassword(userEntity, user.Password);

            var result = await _userManager.CreateAsync(userEntity);
            var addedUserEntity = await _userManager.FindByEmailAsync(user.Email);
            var addedUserRoles = await _userManager.GetRolesAsync(addedUserEntity);

            if (result == IdentityResult.Success && addedUserRoles.IsNullOrEmpty())
                await _userManager.AddToRoleAsync(addedUserEntity, "User");

            var configModel = new MapperConfiguration(cfg => cfg.CreateMap<UserRegistrationRequestModel, UserResponseModel>());
            var mapperModel = new Mapper(configModel);
            var userModel = mapperModel.Map<UserRegistrationRequestModel, UserResponseModel>(user);

            return userModel;
        }

        public async Task<ClaimsIdentity> GetIdentityAsync(string email, string password)
        {
            var userEntity =
                await _userManager.Users.FirstOrDefaultAsync(user =>
                    user.Email == email);

            if (userEntity != null && _passwordHasher.VerifyHashedPassword(userEntity, userEntity.PasswordHash, password) == PasswordVerificationResult.Success)
            {
                await _userManager.GetRolesAsync(userEntity);
                var claims = new List<Claim>
                {
                    // this guarantees the token is unique
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                //Adds all roles to claims
                foreach (var role in await _userManager.GetRolesAsync(userEntity))
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

                return claimsIdentity;
            }

            return null;
        }
    }
}
