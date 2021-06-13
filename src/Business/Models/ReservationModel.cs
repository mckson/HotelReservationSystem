using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelReservation.Business.Models
{
    public class ReservationModel
    {
        public int Id { get; set; }

        public int HotelId { get; set; }

        public HotelModel Hotel { get; set; }

        // public UserModel User { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public List<ReservationRoomModel> ReservationRooms { get; set; }

        public List<ReservationServiceModel> ReservationServices { get; set; }

        public DateTime ReservedTime { get; set; }

        public DateTime DateIn { get; set; }

        public DateTime DateOut { get; set; }

        public int TotalDays => (DateOut - DateIn).Days;

        public double? Deposit => Hotel?.Deposit;

        public double? TotalPrice => Hotel?.Deposit +
                                    ReservationRooms?.Select(rr => rr.Room?.Price).Sum() +
                                    ReservationServices?.Select(rs => rs.Service?.Price).Sum();
    }
}
