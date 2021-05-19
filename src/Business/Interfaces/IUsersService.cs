using HotelReservation.Business.Models;
using HotelReservation.Business.Models.UserModels;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelReservation.Business.Interfaces
{
    public interface IUsersService
    {
        Task<IEnumerable<UserModel>> GetAllUsersAsync();

        Task<UserModel> CreateAsync(UserRegistrationModel userModel);

        Task<UserModel> GetAsync(string id);

        Task<UserModel> DeleteAsync(string id, IEnumerable<Claim> currentUserClaims);

        Task<UserModel> UpdateAsync(string id, UserUpdateModel updatingUserUpdateModel, IEnumerable<Claim> currentUserClaims);
    }
}
