using HotelReservation.Business.Models;
using HotelReservation.Business.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelReservation.Business.Interfaces
{
    public interface IUsersService
    {
        Task<IEnumerable<UserModel>> GetAllUsersAsync();

        Task<UserModel> CreateAsync(UserRegistrationModel userModel);

        Task<UserModel> GetAsync(Guid id);

        Task<bool> DeleteAsync(Guid id, IEnumerable<Claim> currentUserClaims);

        Task<UserModel> UpdateAsync(Guid id, UserUpdateModel updatingUserUpdateModel, IEnumerable<Claim> currentUserClaims);
    }
}
