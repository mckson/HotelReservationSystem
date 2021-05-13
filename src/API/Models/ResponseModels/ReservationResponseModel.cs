using System;
using System.Collections.Generic;

namespace HotelReservation.API.Models.ResponseModels
{
    public class ReservationResponseModel
    {
        public int Id { get; set; }

        public int HotelId { get; set; }

        public string HotelName { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public IEnumerable<RoomResponseModel> Rooms { get; set; }

        public IEnumerable<ServiceResponseModel> Services { get; set; }

        public DateTime ReservedTime { get; set; }

        public DateTime UpdatedTime { get; set; }

        public DateTime DateIn { get; set; }

        public DateTime DateOut { get; set; }

        public int TotalDays { get; set; }

        public double Deposit { get; set; }

        public double TotalPrice { get; set; }
    }
}
