using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
    public class RoomEntity
    {
        public int Id { get; set; }

        public int RoomNumber { get; set; }

        public int FloorNumber { get; set; }

        public int Capacity { get; set; }

        public double Price { get; set; }

        public int HotelId { get; set; }

        public virtual HotelEntity Hotel { get; set; }

        public virtual IEnumerable<ReservationRoomEntity> ReservationRooms { get; set; }
    }
}