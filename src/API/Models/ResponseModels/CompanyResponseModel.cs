using System.Collections.Generic;

namespace HotelReservation.API.Models.ResponseModels
{
    public class CompanyResponseModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public IEnumerable<HotelResponseModel> Hotels { get; set; }
    }
}
