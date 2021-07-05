using System;
using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
    public class ReservationEntity : Entity
    {
        public Guid? HotelId { get; set; }

        public virtual HotelEntity Hotel { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public virtual IEnumerable<ReservationRoomEntity> ReservationRooms { get; set; }

        public virtual IEnumerable<ReservationServiceEntity> ReservationServices { get; set; }

        public DateTime ReservedTime { get; set; }

        public DateTime DateIn { get; set; }

        public DateTime DateOut { get; set; }
    }
}
