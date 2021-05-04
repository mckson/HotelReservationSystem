using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
    public class RoomEntity
    {
        public int Id { get; set; }

        public int RoomNumber { get; set; }

        public int FloorNumber { get; set; }

        public int Capacity { get; set; }

        public bool IsEmpty { get; set; }

        public int HotelId { get; set; }

        public virtual HotelEntity Hotel { get; set; }

        public virtual ReservationEntity Reservation { get; set; }

        public virtual IEnumerable<UserEntity> Users { get; set; }
    }
}