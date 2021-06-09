using System;

namespace HotelReservation.Data.Entities
{
    public class HotelUserEntity
    {
        public int HotelId { get; set; }

        public virtual HotelEntity Hotel { get; set; }

        public Guid UserId { get; set; }

        public virtual UserEntity User { get; set; }
    }
}
