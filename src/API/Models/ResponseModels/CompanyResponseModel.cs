using System.Collections.Generic;

namespace HotelReservation.API.Models.ResponseModels
{
    public class CompanyResponseModel
    {
        public string CompanyName { get; set; }

        public IEnumerable<string> HotelsUrls { get; set; }
    }
}
