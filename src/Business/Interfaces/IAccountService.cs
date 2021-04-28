using System.Security.Claims;
using System.Threading.Tasks;
using HotelReservation.Business.Models;

namespace HotelReservation.Business.Interfaces
{
    public interface IAccountService
    {
        Task<UserModel> AuthenticateAsync(string email, string password);
        Task RegisterAsync(UserModel user, string password);
        Task<ClaimsIdentity> GetIdentityAsync(string email, string password);
    }
}
