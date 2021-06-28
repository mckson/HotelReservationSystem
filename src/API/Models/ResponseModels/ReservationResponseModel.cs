using System;
using System.Collections.Generic;

namespace HotelReservation.API.Models.ResponseModels
{
    public class ReservationResponseModel
    {
        public Guid Id { get; set; }

        public HotelBriefResponse Hotel { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public IEnumerable<RoomBriefResponseModel> Rooms { get; set; }

        public IEnumerable<ServiceBriefResponseModel> Services { get; set; }

        public double RoomsPrice { get; set; }

        public double ServicesPrice { get; set; }

        public DateTime ReservedTime { get; set; }

        public DateTime DateIn { get; set; }

        public DateTime DateOut { get; set; }

        public int TotalDays { get; set; }

        public double Deposit { get; set; }

        public double TotalPrice { get; set; }
    }
}
