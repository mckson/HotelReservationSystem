using System;
using HotelReservation.Business.Constants;
using HotelReservation.Data;
using HotelReservation.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservation.API.Helpers
{
    public class DatabaseSeeder
    {
        private readonly AdminOptions _adminOptions;
        private readonly HotelContext _hotelContext;
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<RoleEntity> _roleManager;

        public DatabaseSeeder(
            IOptions<AdminOptions> adminOptions,
            HotelContext hotelContext,
            UserManager<UserEntity> userManager,
            RoleManager<RoleEntity> roleManager)
        {
            _adminOptions = adminOptions.Value;
            _hotelContext = hotelContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SetupDatabaseAsync()
        {
            await MigrateDatabaseAsync();
            await SeedRolesAsync();
            await SeedAdminCredentialsAsync();
        }

        private async Task MigrateDatabaseAsync()
        {
            await _hotelContext.Database.MigrateAsync();
        }

        private async Task SeedAdminCredentialsAsync()
        {
            var adminLogin = _adminOptions.Email;
            var adminPassword = _adminOptions.Password;
            var adminName = _adminOptions.FirstName;

            if (!_hotelContext.Users.Any(u => u.UserName == adminLogin))
            {
                var admin = new UserEntity
                {
                    UserName = adminLogin,
                    Email = adminLogin,
                    FirstName = adminName,
                    LastName = adminName
                };
                await _userManager.CreateAsync(admin, adminPassword);
                await _userManager.AddToRoleAsync(admin, Roles.Admin);
            }
            else
            {
                var admin = await _userManager.FindByEmailAsync(adminLogin);
                var roles = await _userManager.GetRolesAsync(admin);

                if (!roles.Any(r => r.Equals(Roles.Admin, StringComparison.InvariantCultureIgnoreCase)))
                {
                    await _userManager.AddToRoleAsync(admin, Roles.Admin);
                }
            }
        }

        private async Task SeedRolesAsync()
        {
            var adminRole = new RoleEntity(Roles.Admin);
            var managerRole = new RoleEntity(Roles.Manager);
            var userRole = new RoleEntity(Roles.User);

            if (!_hotelContext.Roles.Any(r => r.Name == adminRole.Name))
                await _roleManager.CreateAsync(adminRole);

            if (!_hotelContext.Roles.Any(r => r.Name == managerRole.Name))
                await _roleManager.CreateAsync(managerRole);

            if (!_hotelContext.Roles.Any(r => r.Name == userRole.Name))
                await _roleManager.CreateAsync(userRole);
        }
    }
}
