using System;

namespace HotelReservation.API.Models.ResponseModels
{
    public class RoomBriefResponseModel
    {
        public Guid Id { get; set; }

        public int RoomNumber { get; set; }

        public double Price { get; set; }
    }
}
