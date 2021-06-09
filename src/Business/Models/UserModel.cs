using System;
using System.Collections.Generic;

namespace HotelReservation.Business.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string PhoneNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int? RoomId { get; set; }

        public virtual RoomModel Room { get; set; }

        // public int? HotelId { get; set; }
        // public HotelModel Hotel { get; set; }
        public IEnumerable<HotelUserModel> HotelUsers { get; set; }

        public IEnumerable<ReservationModel> Reservations { get; set; }

        public RefreshTokenModel RefreshToken { get; set; }

        public string JwtToken { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}