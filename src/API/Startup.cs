using AutoMapper;
using FluentValidation;
using HotelReservation.API.Application.Behaviors;
using HotelReservation.API.Extensions;
using HotelReservation.API.Helpers;
using HotelReservation.API.Middleware;
using HotelReservation.API.Options;
using HotelReservation.Business;
using HotelReservation.Business.Constants;
using HotelReservation.Business.Interfaces;
using HotelReservation.Data;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.IO;
using System.Reflection;
using PasswordOptions = HotelReservation.API.Options.PasswordOptions;

namespace HotelReservation.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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

            services.AddMediatR(typeof(Startup));
            services.AddValidatorsFromAssembly(typeof(Startup).Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Hotel Reservation System API"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);
            });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DatabaseSeeder databaseSeeder)
        {
            databaseSeeder.SetupDatabaseAsync().GetAwaiter().GetResult();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hotel Reservation API V1");
            });

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
