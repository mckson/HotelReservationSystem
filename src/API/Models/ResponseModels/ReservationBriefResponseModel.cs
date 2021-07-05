using System;

namespace HotelReservation.API.Models.ResponseModels
{
    public class ReservationBriefResponseModel
    {
        public Guid Id { get; set; }

        public string HotelName { get; set; }

        public DateTime DateIn { get; set; }

        public int TotalDays { get; set; }

        public double? TotalPrice { get; set; }
    }
}
