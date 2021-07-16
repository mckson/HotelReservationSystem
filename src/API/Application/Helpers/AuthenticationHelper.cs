using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HotelReservation.API.Application.Interfaces;
using HotelReservation.Business;
using HotelReservation.Business.Constants;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Interfaces;
using Microsoft.AspNetCore.Http;

namespace HotelReservation.API.Application.Helpers
{
    public class AuthenticationHelper : IAuthenticationHelper
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationHelper(
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
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

        public bool CheckGetUserPermission(Guid userId)
        {
            var userClaims = _httpContextAccessor.HttpContext.User.Claims;

            var claims = userClaims.ToList();
            if (claims.Where(claim => claim.Type.Equals(ClaimTypes.Role))
                .Any(role => role.Value.Equals(Roles.Admin, StringComparison.InvariantCultureIgnoreCase)))
            {
                return true;
            }

            var currentUserIdString = claims.Find(claim => claim.Type.Equals(ClaimNames.Id))?.Value ??
                                      throw new BusinessException("User is unauthorized", ErrorStatus.AccessDenied);

            var currentUserId = Guid.Parse(currentUserIdString);

            return currentUserId.Equals(userId);
        }
    }
}
