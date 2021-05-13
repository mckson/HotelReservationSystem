using System.Collections.Generic;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;

namespace HotelReservation.Business.Interfaces
{
    public interface IReservationsService : IBaseService<ReservationEntity, ReservationModel>
    {
        IEnumerable<ReservationModel> GetAllReservations();
    }
}
