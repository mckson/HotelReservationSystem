using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using HotelReservation.Business.Models.UserModels;
using HotelReservation.Data.Entities;

namespace HotelReservation.Business.Interfaces
{
    public interface IUsersService
    {
        Task<IEnumerable<UserModel>> GetAllUsersAsync();

        Task<UserModel> CreateAsync(UserRegistrationModel userModel);

        Task<UserModel> GetAsync(string id);

        Task<UserModel> DeleteAsync(string id, IEnumerable<Claim> currentUserClaims);

        Task<UserModel> UpdateAsync(string id, UserUpdateModel model, IEnumerable<Claim> currentUserClaims);
    }
}
