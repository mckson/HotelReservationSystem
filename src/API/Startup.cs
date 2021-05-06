using System;
using System.Text;
using AutoMapper;
using HotelReservation.Business;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Services;
using HotelReservation.Data;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using HotelReservation.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace HotelReservation.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<HotelContext>(opt =>
            {
                opt.UseLazyLoadingProxies();
                opt.UseSqlServer(Configuration.GetConnectionString("HotelContextConnection"));
            });

            services.AddIdentityCore<UserEntity>(options => { });
            new IdentityBuilder(typeof(UserEntity), typeof(IdentityRole), services)
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddSignInManager<SignInManager<UserEntity>>()
                .AddUserManager<UserManager<UserEntity>>()
                .AddEntityFrameworkStores<HotelContext>();

            services.AddScoped<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();

            services.AddScoped<IAccountService, AccountService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["AuthOptions:issuer"],

                        ValidateAudience = true,
                        ValidAudience = Configuration["AuthOptions:audience"],

                        ValidateLifetime = true,

                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["AuthOptions:key"])),
                        ValidateIssuerSigningKey = true
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "GetHotelsPermission",
                    policy =>
                    {
                        policy.RequireAuthenticatedUser();
                    });

                options.AddPolicy(
                    "PostHotelsPermission",
                    policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Admin", "Manager");
                });

                options.AddPolicy(
                    "UpdateHotelsPermission",
                    policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireRole("Admin", "Manager");
                    });
            });

            services.AddScoped<IRepository<HotelEntity>, HotelRepository>();
            services.AddScoped<IRepository<CompanyEntity>, CompanyRepository>();
            services.AddScoped<IRepository<LocationEntity>, LocationRepository>();

            // var mappingConfig = new MapperConfiguration(config =>
            // {
            //     config.AddProfile(new ModelEntityMapperProfile());
            //     config.AddProfile(new ModelToModelApiProfile());
            // });
            // var mapper = mappingConfig.CreateMapper();
            // services.AddSingleton(mapper);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            /*services.AddSingleton<IMapper<LocationEntity, LocationResponseModel, LocationRequestModel>, LocationMapper>();
            services.AddSingleton<IMapper<RoomEntity, RoomResponseModel, RoomRequestModel>, RoomMapper>();
            services.AddSingleton<IMapper<HotelEntity, HotelResponseModel, HotelRequestModel>, HotelMapper>();
            services.AddSingleton<IMapper<CompanyEntity, CompanyResponseModel, CompanyRequestModel>, CompanyMapper>();*/

            services.AddScoped<IHotelsService, HotelsService>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            // Write streamlined request completion events, instead of the more verbose ones from the framework.
            // To use the default framework request logging instead, remove this line and set the "Microsoft"
            // level in appsettings.json to "Information".
            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
