using System;

namespace HotelReservation.API.Models.ResponseModels
{
    public class ImageResponseModel
    {
        public Guid Id { get; set; }

        public string Image { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public Guid HotelId { get; set; }
    }
}
