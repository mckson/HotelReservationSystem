using HotelReservation.API.Extensions;
using HotelReservation.API.Helpers;
using HotelReservation.API.Middleware;
using HotelReservation.Business;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Services;
using HotelReservation.Data;
using HotelReservation.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Text;

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

            services.AddIdentity<UserEntity, RoleEntity>()
                .AddEntityFrameworkStores<HotelContext>()
                .AddUserManager<UserManager<UserEntity>>()
                .AddRoleManager<RoleManager<RoleEntity>>();

            services.Configure<AuthenticationOptions>(Configuration.GetSection(AuthenticationOptions.Authentication));
            services.Configure<AdminOptions>(Configuration.GetSection(AdminOptions.AdminCredentials));

            // services.AddIdentityCore<UserEntity>(options =>
            // {
            //     options.Password.RequireDigit = false;
            //     options.Password.RequiredLength = 8;
            //     options.Password.RequireNonAlphanumeric = false;
            //     options.Password.RequireUppercase = false;
            //     options.Password.RequireLowercase = false;
            // });
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "ApiCorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });

            services.AddScoped<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAccountService, AccountService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var authOptions = services.BuildServiceProvider()
                        .GetRequiredService<IOptions<AuthenticationOptions>>()
                        .Value;

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
                    "AdminPermission",
                    policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Admin");
                });

                options.AddPolicy(
                    "AdminManagerPermission",
                    policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireRole("Admin", "Manager");
                    });

                options.AddPolicy(
                    "UserPermission",
                    policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireRole("Admin", "User");
                    });
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDataAndBusiness();

            services.AddScoped<ApplicationAdminSeeder>();

            // SeedAdminCredentials(services);
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationAdminSeeder applicationAdminSeeder)
        {
            applicationAdminSeeder.SeedCredentialsAsync().GetAwaiter().GetResult();

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

            app.UseCors("ApiCorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        // private void SeedAdminCredentials(IServiceCollection services)
        // {
        //     using var serviceProvider = services.BuildServiceProvider();
        //     var applicationAdminSeeder = serviceProvider.GetRequiredService<ApplicationAdminSeeder>();
        //     applicationAdminSeeder.SeedCredentials();
        // }
    }
}
