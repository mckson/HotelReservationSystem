using System;
using System.Collections.Generic;

namespace HotelReservation.Business.Models
{
    public class ReservationModel
    {
        public int Id { get; set; }

        public int HotelId { get; set; }

        public HotelModel Hotel { get; set; }

        public string UserId { get; set; }

        public UserModel User { get; set; }

        public IEnumerable<ReservationRoomModel> ReservationRooms { get; set; }

        public IEnumerable<ReservationServiceModel> ReservationServices { get; set; }

        public DateTime ReservedTime { get; set; }

        public DateTime UpdatedTime { get; set; }

        public DateTime DateIn { get; set; }

        public DateTime DateOut { get; set; }

        public int TotalDays { get; set; }

        public double Deposit { get; set; }

        public double TotalPrice { get; set; }
    }
}
