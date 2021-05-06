using System;

namespace HotelReservation.Business.Models.UserModels
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

        public int? HotelId { get; set; }

        public virtual HotelModel Hotel { get; set; }

        public virtual ReservationModel Reservation { get; set; }
    }
}