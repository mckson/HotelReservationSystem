using System;

namespace HotelReservation.Data.Entities
{
    public class HotelImageEntity : ImageEntity
    {
        public bool IsMain { get; set; }

        public Guid HotelId { get; set; }

        public virtual HotelEntity Hotel { get; set; }
    }
}
