using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Castle.Core.Internal;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Business.Models.UserModels;
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
        private readonly IMapper _mapper;

        public AccountService(
            UserManager<UserEntity> userManager,
            IPasswordHasher<UserEntity> passwordHasher,
            IMapper mapper)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<UserModel> AuthenticateAsync(UserAuthenticationModel user)
        {
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
                throw new DataException("Password and email cannot be empty", ErrorStatus.EmptyInput);

            var userEntity = await _userManager.FindByEmailAsync(user.Email) ??
                             throw new DataException("User with such email does not exist", ErrorStatus.NotFound);

            if (_passwordHasher.VerifyHashedPassword(userEntity, userEntity.PasswordHash, user.Password) ==
                PasswordVerificationResult.Failed)
                throw new DataException("Incorrect password", ErrorStatus.IncorrectInput);

            return _mapper.Map<UserModel>(userEntity);
        }

        public async Task<UserAuthenticationModel> RegisterAsync(UserRegistrationModel userRegistration)
        {
            if (userRegistration == null)
                throw new DataException("User cannot be empty", ErrorStatus.EmptyInput);

            var existingUserEntity = await _userManager.FindByEmailAsync(userRegistration.Email);

            if (existingUserEntity != null)
                throw new DataException("User with such email already exists", ErrorStatus.AlreadyExisted);

            var userEntity = _mapper.Map<UserEntity>(userRegistration);

            userEntity.UserName ??= userRegistration.Email.Split('@', StringSplitOptions.RemoveEmptyEntries)[0];
            userEntity.PasswordHash = _passwordHasher.HashPassword(userEntity, userRegistration.Password);

            var result = await _userManager.CreateAsync(userEntity);
            var addedUserEntity = await _userManager.FindByEmailAsync(userRegistration.Email);
            var addedUserRoles = await _userManager.GetRolesAsync(addedUserEntity);

            if (result == IdentityResult.Success && addedUserRoles.IsNullOrEmpty())
                await _userManager.AddToRoleAsync(addedUserEntity, "User");

            return _mapper.Map<UserAuthenticationModel>(userRegistration);
        }

        public async Task<ClaimsIdentity> GetIdentityAsync(UserAuthenticationModel userAuth)
        {
            var userEntity =
                await _userManager.Users.FirstOrDefaultAsync(user =>
                    user.Email == userAuth.Email) ??
                throw new DataException("There is no user with such email", ErrorStatus.NotFound);

            if (userAuth.Password == null ||
                _passwordHasher.VerifyHashedPassword(userEntity, userEntity.PasswordHash, userAuth.Password) !=
                PasswordVerificationResult.Success)
                throw new DataException("Wrong password", ErrorStatus.IncorrectInput);

            var claims = new List<Claim>
            {
                // this guarantees the token is unique
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Adds all roles to claims
            foreach (var role in await _userManager.GetRolesAsync(userEntity))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims,
                "Token",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}
