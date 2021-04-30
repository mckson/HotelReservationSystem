using HotelReservation.Data;
using HotelReservation.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservation.API
{
    public class Program
    {
        public static async Task Main(string[] args)
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

                    await dbContext.Database.MigrateAsync();

                    var adminLogin = configuration["AdminLogin:Email"];
                    var adminPassword = configuration["AdminLogin:Password"];
                    var adminName = configuration["AdminLogin:FirstName"];

                    var adminRole = new IdentityRole("Admin");
                    var managerRole = new IdentityRole("Manager");
                    var userRole = new IdentityRole("User");

                    if (!dbContext.Roles.Any())
                    {
                        await roleManager.CreateAsync(adminRole);
                        await roleManager.CreateAsync(managerRole);
                        await roleManager.CreateAsync(userRole);
                    }
                    else
                    {
                        if (!dbContext.Roles.Any(r => r.Name == adminRole.Name))
                            await roleManager.CreateAsync(adminRole);
                        if (!dbContext.Roles.Any(r => r.Name == managerRole.Name))
                            await roleManager.CreateAsync(managerRole);
                        if (!dbContext.Roles.Any(r => r.Name == userRole.Name))
                            await roleManager.CreateAsync(userRole);
                    }

                    if (!dbContext.Users.Any(u => u.UserName == adminLogin))
                    {
                        var admin = new UserEntity
                        {
                            UserName = adminLogin,
                            Email = adminLogin,
                            FirstName = adminName,
                            LastName = adminName
                        };
                        await userManger.CreateAsync(admin, adminPassword);
                        await userManger.AddToRoleAsync(admin, adminRole.Name);
                    }
                }
                await host.RunAsync();
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
