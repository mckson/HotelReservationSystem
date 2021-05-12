﻿using AutoMapper;
using Castle.Core.Internal;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Business.Models.UserModels;
using HotelReservation.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelReservation.Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IPasswordHasher<UserEntity> _passwordHasher;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly ILogger _logger;

        public AccountService(
            UserManager<UserEntity> userManager,
            IPasswordHasher<UserEntity> passwordHasher,
            ITokenService tokenService,
            IMapper mapper,
            ILogger logger)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserModel> AuthenticateAsync(UserAuthenticationModel user)
        {
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
                throw new BusinessException("Password and email cannot be empty", ErrorStatus.EmptyInput);

            var userEntity = await _userManager.FindByEmailAsync(user.Email) ??
                             throw new BusinessException("User with such email does not exist", ErrorStatus.NotFound);

            if (_passwordHasher.VerifyHashedPassword(userEntity, userEntity.PasswordHash, user.Password) ==
                PasswordVerificationResult.Failed)
                throw new BusinessException("Incorrect password", ErrorStatus.IncorrectInput);

            var claims = await GetIdentityAsync(user);

            var encodedJwt = _tokenService.GenerateJwtToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();
            var refreshTokenEntity = _mapper.Map<RefreshTokenEntity>(refreshToken);
            userEntity.RefreshToken = refreshTokenEntity;
            await _userManager.UpdateAsync(userEntity);

            var userModel = _mapper.Map<UserModel>(userEntity);
            userModel.JwtToken = encodedJwt;

            await GetRolesForUserModelAsync(userModel);

            return userModel;
        }

        public async Task<UserAuthenticationModel> RegisterAsync(UserRegistrationModel userRegistration)
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
                await _userManager.AddToRoleAsync(addedUserEntity, "User");

            return _mapper.Map<UserAuthenticationModel>(userRegistration);
        }

        public async Task<UserModel> RefreshToken(string token)
        {
            var userEntity = _userManager.Users.FirstOrDefault(u => u.RefreshToken.Token == token);

            if (userEntity == null)
                return null;

            var refreshToken = userEntity.RefreshToken;

            if (!refreshToken.IsActive)
                return null;

            var newRefreshToken = _tokenService.GenerateRefreshToken();
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            userEntity.RefreshToken = _mapper.Map<RefreshTokenEntity>(newRefreshToken);

            await _userManager.UpdateAsync(userEntity);

            var user = _mapper.Map<UserModel>(userEntity);

            var claims = await GetIdentityAsync(_mapper.Map<UserAuthenticationModel>(user));
            var jwtToken = _tokenService.GenerateJwtToken(claims);

            var userModel = _mapper.Map<UserModel>(user);
            userModel.JwtToken = jwtToken;

            await GetRolesForUserModelAsync(userModel);

            return userModel;
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken.Token == token);
            var userModel = _mapper.Map<UserModel>(user);

            // return false if no user found with token
            if (userModel == null)
                return false;

            var refreshToken = userModel.RefreshToken;

            // return false if token is not active
            if (!refreshToken.IsActive)
                return false;

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;

            await _userManager.UpdateAsync(_mapper.Map<UserEntity>(userModel));

            return true;
        }

        private async Task<ClaimsIdentity> GetIdentityAsync(UserAuthenticationModel userAuth)
        {
            var userEntity =
                await _userManager.Users.FirstOrDefaultAsync(user =>
                    user.Email == userAuth.Email) ??
                throw new BusinessException("There is no user with such email", ErrorStatus.NotFound);

            // if (userAuth.Password == null ||
            //    _passwordHasher.VerifyHashedPassword(userEntity, userEntity.PasswordHash, userAuth.Password) !=
            //    PasswordVerificationResult.Success)
            //    throw new DataException("Wrong password", ErrorStatus.IncorrectInput);
            var claims = new List<Claim>
            {
                // this guarantees the token is unique
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", userEntity.Id),
                new Claim("hotelId", userEntity.HotelId.HasValue ? userEntity.HotelId.Value.ToString() : "0")
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

        private async Task GetRolesForUserModelAsync(UserModel userModel)
        {
            var roles = await _userManager.GetRolesAsync(_mapper.Map<UserEntity>(userModel));
            userModel.Roles = roles;
        }
    }
}
