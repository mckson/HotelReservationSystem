using System;

namespace HotelReservation.API.Models.ResponseModels
{
    public class RoomFacilityResponseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid RoomId { get; set; }
    }
}
