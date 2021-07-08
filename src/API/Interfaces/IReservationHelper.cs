using HotelReservation.Data.Entities;
using System.Threading.Tasks;

namespace HotelReservation.API.Interfaces
{
    public interface IReservationHelper
    {
        Task CheckHotelRoomsServicesExistenceAsync(ReservationEntity reservationEntity);
    }
}
