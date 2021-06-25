using HotelReservation.Business.Models;
using System.Collections.Generic;

namespace HotelReservation.Business.Interfaces
{
    public interface IReservationsService : IBaseService<ReservationModel>
    {
        IEnumerable<ReservationModel> GetAllReservations();

        IEnumerable<ReservationModel> GetReservationsByEmail(string email);
    }
}
