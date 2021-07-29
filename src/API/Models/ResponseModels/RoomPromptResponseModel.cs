using System;

namespace HotelReservation.API.Models.ResponseModels
{
    public class RoomPromptResponseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Number { get; set; }
    }
}
