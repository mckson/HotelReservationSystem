using HotelReservation.Business.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace HotelReservation.Business.Interfaces
{
    public interface IReservationsService : IBaseService<ReservationModel>
    {
        IEnumerable<ReservationModel> GetAllReservations(IEnumerable<Claim> userClaims);

        IEnumerable<ReservationModel> GetReservationsByEmail(string email);
    }
}
