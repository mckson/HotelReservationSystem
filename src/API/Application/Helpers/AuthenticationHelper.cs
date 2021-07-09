using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HotelReservation.API.Application.Interfaces;
using HotelReservation.Business;
using HotelReservation.Business.Constants;
using HotelReservation.Data.Interfaces;

namespace HotelReservation.API.Application.Helpers
{
    public class AuthenticationHelper : IAuthenticationHelper
    {
        private readonly IUserRepository _userRepository;

        public AuthenticationHelper(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ClaimsIdentity> GetIdentityAsync(string email)
        {
            var userEntity =
                await _userRepository.GetByEmailAsync(email) ??
                throw new BusinessException("There is no user with such email", ErrorStatus.NotFound);

            var claims = new List<Claim>
            {
                new Claim(ClaimNames.Id, userEntity.Id.ToString()),
                new Claim(ClaimNames.Name, userEntity.UserName),
                new Claim(ClaimNames.FirstName, userEntity.FirstName),
                new Claim(ClaimNames.LastName, userEntity.LastName),
                new Claim(ClaimNames.Email, userEntity.Email)
            };

            // Adds all roles to claims
            foreach (var role in userEntity.Roles)
            {
                claims.Add(new Claim(ClaimNames.Roles, role));
            }

            if (userEntity.HotelUsers != null)
            {
                claims.AddRange(userEntity.HotelUsers.Select(hotelUsers =>
                    new Claim(ClaimNames.Hotels, hotelUsers.HotelId.ToString())));
            }

            var claimsIdentity = new ClaimsIdentity(
                claims,
                "Token");

            return claimsIdentity;
        }
    }
}
