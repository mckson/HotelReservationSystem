using System;

namespace HotelReservation.API.Models.ResponseModels
{
    public class RoomImageResponseModel
    {
        public Guid Id { get; set; }

        public string Image { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public Guid RoomId { get; set; }
    }
}
