using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;

namespace HotelReservation.Business.Interfaces
{
    public interface IReservationService : IBaseService<ReservationEntity, ReservationModel>
    {
    }
}
