using HotelReservation.API.Interfaces;
using HotelReservation.Business;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservation.API.Helpers
{
    public class ReservationHelper : IReservationHelper
    {
        private readonly IHotelRepository _hotelRepository;

        public ReservationHelper(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task CheckHotelRoomsServicesExistenceAsync(ReservationEntity reservationEntity)
        {
            if (reservationEntity.HotelId != null)
            {
                var checkHotelEntity = await _hotelRepository.GetAsync(reservationEntity.HotelId.Value) ??
                                       throw new BusinessException($"No hotel with such id {reservationEntity.HotelId}", ErrorStatus.NotFound);

                foreach (var room in reservationEntity.ReservationRooms)
                {
                    var checkRoomEntity = checkHotelEntity.Rooms.FirstOrDefault(r => r.Id == room.RoomId) ??
                                          throw new BusinessException($"No room with such id: {room.RoomId}", ErrorStatus.NotFound);

                    var checkReservation = checkRoomEntity.ReservationRooms.Select(rr => rr.Reservation).FirstOrDefault(
                        reservation =>
                            (reservation.DateIn >= reservationEntity.DateIn && reservation.DateIn < reservationEntity.DateOut) ||
                            (reservation.DateOut > reservationEntity.DateIn && reservation.DateOut <= reservationEntity.DateOut));

                    if (checkReservation != null)
                    {
                        throw new BusinessException(
                            $"Room with id {checkRoomEntity.Id} has been already reserved from {checkReservation.DateIn} to {checkReservation.DateOut}",
                            ErrorStatus.IncorrectInput);
                    }
                }

                foreach (var service in reservationEntity.ReservationServices)
                {
                    var unused = checkHotelEntity.Services.FirstOrDefault(s => s.Id == service.ServiceId) ??
                                 throw new BusinessException(
                                     $"No service with such id: {service.ServiceId}",
                                     ErrorStatus.NotFound);
                }
            }
            else
            {
                throw new BusinessException("Reservation has no hotel id", ErrorStatus.NotFound);
            }
        }
    }
}
