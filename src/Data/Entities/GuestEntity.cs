using System;
using Microsoft.AspNetCore.Identity;

namespace HotelReservation.Data.Entities
{
    public class GuestEntity : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public int RoomId { get; set; }
        public RoomEntity Room { get; set; }

        public int HotelId { get; set; }
        public HotelEntity Hotel { get; set; }

        public ReservationEntity Reservation { get; set; }
    }
}
