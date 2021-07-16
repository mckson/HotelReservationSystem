using System.Threading.Tasks;
using HotelReservation.Data.Entities;

namespace HotelReservation.API.Application.Interfaces
{
    public interface IReservationHelper
    {
        Task CheckHotelRoomsServicesExistenceAsync(ReservationEntity reservationEntity);
    }
}
