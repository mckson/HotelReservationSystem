using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using HotelReservation.Business.Models;

namespace HotelReservation.Business.Interfaces
{
    public interface IReservationsService
    {
        Task<ReservationModel> CreateAsync(ReservationModel userModel, IEnumerable<Claim> userClaims);

        Task<ReservationModel> GetAsync(int id, IEnumerable<Claim> userClaims);

        Task<ReservationModel> DeleteAsync(int id, IEnumerable<Claim> userClaims);

        Task<IEnumerable<ReservationModel>> GetAllReservationsAsync(IEnumerable<Claim> userClaims);
    }
}
