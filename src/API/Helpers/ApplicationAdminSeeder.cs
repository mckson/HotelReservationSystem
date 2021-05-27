using HotelReservation.Data;
using HotelReservation.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservation.API.Helpers
{
    public class ApplicationAdminSeeder
    {
        private readonly AdminOptions _adminOptions;
        private readonly HotelContext _hotelContext;
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<RoleEntity> _roleManager;

        public ApplicationAdminSeeder(
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

        public async Task SeedCredentialsAsync()
        {
            // startup migration - extract into separate method?
            _hotelContext.Database.Migrate();

            var adminLogin = _adminOptions.Email;
            var adminPassword = _adminOptions.Password;
            var adminName = _adminOptions.FirstName;

            var adminRole = new RoleEntity { Name = "Admin" };
            var managerRole = new RoleEntity { Name = "Manager" };
            var userRole = new RoleEntity { Name = "User" };

            if (!_hotelContext.Roles.Any())
            {
                await _roleManager.CreateAsync(adminRole);
                await _roleManager.CreateAsync(managerRole);
                await _roleManager.CreateAsync(userRole);
            }
            else
            {
                if (!_hotelContext.Roles.Any(r => r.Name == adminRole.Name))
                    await _roleManager.CreateAsync(adminRole);
                if (!_hotelContext.Roles.Any(r => r.Name == managerRole.Name))
                    await _roleManager.CreateAsync(managerRole);
                if (!_hotelContext.Roles.Any(r => r.Name == userRole.Name))
                    await _roleManager.CreateAsync(userRole);
            }

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
                await _userManager.AddToRoleAsync(admin, adminRole.Name);
            }
        }
    }
}
