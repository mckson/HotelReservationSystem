using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using HotelReservation.Business.Models;

namespace HotelReservation.Business.Interfaces
{
    public interface IReservationsService
    {
        Task<ReservationModel> CreateAsync(ReservationModel userModel);

        Task<ReservationModel> GetAsync(int id, IEnumerable<Claim> userClaims);

        Task<ReservationModel> DeleteAsync(int id, IEnumerable<Claim> userClaims);

        IEnumerable<ReservationModel> GetAllReservations(IEnumerable<Claim> userClaims);

        IEnumerable<ReservationModel> GetReservationsByEmail(string email);
    }
}
