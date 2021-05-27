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

        public void SeedCredentials()
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
                _roleManager.CreateAsync(adminRole).GetAwaiter();
                _roleManager.CreateAsync(managerRole).GetAwaiter();
                _roleManager.CreateAsync(userRole).GetAwaiter();
            }
            else
            {
                if (!_hotelContext.Roles.Any(r => r.Name == adminRole.Name))
                    _roleManager.CreateAsync(adminRole).GetAwaiter().GetResult();
                if (!_hotelContext.Roles.Any(r => r.Name == managerRole.Name))
                    _roleManager.CreateAsync(managerRole).GetAwaiter().GetResult();
                if (!_hotelContext.Roles.Any(r => r.Name == userRole.Name))
                    _roleManager.CreateAsync(userRole).GetAwaiter().GetResult();
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
                _userManager.CreateAsync(admin, adminPassword).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(admin, adminRole.Name).GetAwaiter().GetResult();
            }
        }
    }
}
