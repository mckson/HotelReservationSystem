using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Interfaces
{
    public interface IAuthenticationHelper
    {
        Task<ClaimsIdentity> GetIdentityAsync(string email);

        bool CheckGetUserPermission(Guid userId);
    }
}
