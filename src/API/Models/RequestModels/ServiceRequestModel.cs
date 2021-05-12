using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelReservation.API.Models.RequestModels
{
    public class ServiceRequestModel
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public IEnumerable<int> HotelIds { get; set; }
    }
}
