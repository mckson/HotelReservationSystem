using AutoMapper;
using HotelReservation.API.Extensions;
using HotelReservation.API.Helpers;
using HotelReservation.API.Middleware;
using HotelReservation.Business;
using HotelReservation.Business.Constants;
using HotelReservation.Business.Interfaces;
using HotelReservation.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using PasswordOptions = HotelReservation.API.Helpers.PasswordOptions;

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

            // used in token service
            services.Configure<AuthenticationOptions>(Configuration.GetSection(AuthenticationOptions.Authentication));
            services.Configure<AdminOptions>(Configuration.GetSection(AdminOptions.AdminCredentials));

            services.AddAndConfigureIdentity(Configuration.GetSection(PasswordOptions.PasswordSettings).Get<PasswordOptions>());

            services.AddCors(options =>
            {
                var corsOptions = Configuration.GetSection(CorsOptions.CorsSettings).Get<CorsOptions>();

                options.AddPolicy(
                    CorsPolicies.ApiCorsPolicy,
                    builder =>
                    {
                        builder.WithOrigins(corsOptions.AllowedHosts)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });

            services.AddAuthenticationAndAuthorization(Configuration.GetSection(AuthenticationOptions.Authentication).Get<AuthenticationOptions>());

            services.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingApiModelsProfile(provider.GetService<IUriService>()));
                cfg.AddProfile(new ModelEntityMapperProfile());
            }).CreateMapper());
            // services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddDataAndBusiness();

            services.AddScoped<DatabaseSeeder>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DatabaseSeeder databaseSeeder)
        {
            databaseSeeder.SetupDatabaseAsync().GetAwaiter().GetResult();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseCors(CorsPolicies.ApiCorsPolicy);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
