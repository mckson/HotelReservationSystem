using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Business.Models.RequestModels;
using HotelReservation.Data;
using HotelReservation.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManger;
        private readonly HotelContext _db;

        public AccountService(UserManager<UserEntity> userManager, RoleManager<IdentityRole> roleManager, HotelContext context)
        {
            _userManager = userManager;
            _roleManger = roleManager;
            _db = context;
        }

        public async Task<UserModel> AuthenticateAsync(string email, string password)
        {
            if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password))
                return null;

            var userEntity = await _userManager.FindByEmailAsync(email);
            if (userEntity == null)
            {
                return null;
            }
            if (userEntity.PasswordHash != password)
            {
                return null;
            }

            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserEntity, UserModel>()
                .ForMember("Password", opt => opt.MapFrom(c => c.PasswordHash)));
            var mapper = new Mapper(config);

            return mapper.Map<UserEntity, UserModel>(userEntity);

        }

        public async Task<UserModel> RegisterAsync(UserRegistrationRequestModel user, string password)
        {
            if (String.IsNullOrEmpty(password) || user == null)
                return null;

            bool isExistingUser = await _userManager.Users.AnyAsync(userEntity => userEntity.Email == user.Email);

            if (isExistingUser)
                return null;

            //computing hash
            user.Password = password;

            var configEntity = new MapperConfiguration(cfg => cfg.CreateMap<UserRegistrationRequestModel, UserEntity>());
            var mapperEntity = new Mapper(configEntity);

            var userEntity = mapperEntity.Map<UserRegistrationRequestModel, UserEntity>(user);
            userEntity.UserName ??= user.Email.Split('@', StringSplitOptions.RemoveEmptyEntries)[0];
            userEntity.PasswordHash = user.Password; //add hash function here

            var result = await _userManager.CreateAsync(userEntity);

            IEnumerable<IdentityError> errors;
            if (result.Succeeded)
            {

            }
            else
            {
                errors = result.Errors;
            }
            //await _db.SaveChangesAsync();

            var isAdded = await _userManager.FindByEmailAsync(userEntity.Email);

            var configModel = new MapperConfiguration(cfg => cfg.CreateMap<UserRegistrationRequestModel, UserModel>());
            var mapperModel = new Mapper(configModel);
            var userModel = mapperModel.Map<UserRegistrationRequestModel, UserModel>(user);

            return userModel;
        }

        public async Task<ClaimsIdentity> GetIdentityAsync(string email, string password)
        {
            var userEntity =
                await _userManager.Users.FirstOrDefaultAsync(user =>
                    user.Email == email && user.PasswordHash == password);

            if (userEntity != null)
            {
                var roles = await _userManager.GetRolesAsync(userEntity);
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, userEntity.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "User")
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

                return claimsIdentity;
            }

            return null;
        }
    }
}
