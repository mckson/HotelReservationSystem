using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManger;

        public AccountService(UserManager<UserEntity> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManger = roleManager;
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

        public async Task RegisterAsync(UserModel user, string password)
        {
            if (String.IsNullOrEmpty(password) || user == null)
                return;

            bool isExistingUser = await _userManager.Users.AnyAsync(userEntity => userEntity.Email == user.Email);

            if (isExistingUser)
                return;

            //computing hash
            user.Password = password;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserModel, UserEntity>());
            var mapper = new Mapper(config);

            var userEntity = mapper.Map<UserModel, UserEntity>(user);

            await _userManager.CreateAsync(userEntity);
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
