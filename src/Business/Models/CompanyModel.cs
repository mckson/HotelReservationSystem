using System.Collections.Generic;

namespace HotelReservation.Business.Models
{
    public class CompanyModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public virtual IEnumerable<HotelModel> Hotels { get; set; }
    }
}
