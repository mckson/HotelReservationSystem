using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
    public class CompanyEntity
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public virtual IEnumerable<HotelEntity> Hotels { get; set; }
    }
}
