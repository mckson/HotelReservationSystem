using System;
using System.Linq;
using HotelReservation.Data;
using HotelReservation.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace HotelReservation.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Application starting");

                var host = CreateHostBuilder(args).Build();

                using (var services = host.Services.CreateScope())
                {
                    var dbContext = services.ServiceProvider.GetRequiredService<HotelContext>();
                    var userManger = services.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
                    var roleManager = services.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    var configuration = services.ServiceProvider.GetRequiredService<IConfiguration>();

                    dbContext.Database.Migrate();

                    var adminLogin = configuration["AdminLogin:Email"];
                    var adminPassword = configuration["AdminLogin:Password"];
                    var adminName = configuration["AdminLogin:FirstName"];
                    var adminRole = new IdentityRole("Admin");

                    if (!dbContext.Roles.Any() || !dbContext.Roles.Any(r => r.Name == adminRole.Name))
                        roleManager.CreateAsync(adminRole).GetAwaiter().GetResult();

                    if (!dbContext.Users.Any(u => u.UserName == adminLogin))
                    {
                        var admin = new UserEntity
                        {
                            UserName = adminLogin,
                            Email = adminLogin,
                            FirstName = adminName,
                            LastName = adminName
                        };
                        var result = userManger.CreateAsync(admin, adminPassword).GetAwaiter().GetResult();
                        userManger.AddToRoleAsync(admin, adminRole.Name).GetAwaiter().GetResult();
                    }
                }
                host.Run();
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext()
                    .WriteTo.Console())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
