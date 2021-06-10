﻿using HotelReservation.Business;
using HotelReservation.Business.Constants;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Services;
using HotelReservation.Data;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using HotelReservation.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using PasswordOptions = HotelReservation.API.Helpers.PasswordOptions;

namespace HotelReservation.API.Extensions
{
    public static class ConfigureServicesExtensions
    {
        public static IServiceCollection AddDataAndBusiness(this IServiceCollection services)
        {
            services.AddScoped<IHotelRepository, HotelRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IRepository<RoomEntity>, RoomRepository>();
            services.AddScoped<IRepository<ReservationEntity>, ReservationRepository>();
            services.AddScoped<IRepository<ServiceEntity>, ServiceRepository>();
            services.AddScoped<IRepository<ImageEntity>, ImagesRepository>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IHotelsService, HotelsService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IRoomsService, RoomsService>();
            services.AddScoped<IServicesService, ServicesService>();
            services.AddScoped<IReservationsService, ReservationsService>();
            services.AddScoped<IImageService, ImagesService>();

            services.AddHttpContextAccessor();
            services.AddSingleton<IUriService, UriService>(options =>
            {
                var accessor = options.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());

                return new UriService(uri);
            });

            return services;
        }

        public static IServiceCollection AddAndConfigureIdentity(
            this IServiceCollection services,
            PasswordOptions passwordOptions)
        {
            services.AddIdentity<UserEntity, RoleEntity>()
                .AddEntityFrameworkStores<HotelContext>()
                .AddUserManager<UserManager<UserEntity>>()
                .AddRoleManager<RoleManager<RoleEntity>>();

            services.AddIdentityCore<UserEntity>(options =>
            {
                options.Password.RequireDigit = passwordOptions.RequireDigit;
                options.Password.RequiredLength = passwordOptions.RequiredLength;
                options.Password.RequireNonAlphanumeric = passwordOptions.RequireNonAlphanumeric;
                options.Password.RequireUppercase = passwordOptions.RequireUppercase;
                options.Password.RequireLowercase = passwordOptions.RequireLowercase;
            });

            return services;
        }

        public static IServiceCollection AddAuthenticationAndAuthorization(
            this IServiceCollection services,
            AuthenticationOptions authOptions)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authOptions.Issuer,

                        ValidateAudience = true,
                        ValidAudience = authOptions.Audience,

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(30),

                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authOptions.Key)),
                        ValidateIssuerSigningKey = true
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    Policies.AdminPermission,
                    policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireRole(Roles.Admin);
                    });

                options.AddPolicy(
                    Policies.AdminManagerPermission,
                    policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireRole(Roles.Admin, Roles.Manager);
                    });
            });

            return services;
        }
    }
}
