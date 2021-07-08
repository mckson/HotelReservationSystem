using HotelReservation.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
    public class UserEntity : IdentityUser<Guid>, IEntity
    {
        public IEnumerable<string> Roles { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Guid? RoomId { get; set; }

        public virtual RoomEntity Room { get; set; }

        public virtual List<HotelUserEntity> HotelUsers { get; set; }

        public virtual IEnumerable<ReservationEntity> Reservations { get; set; }

        public virtual RefreshTokenEntity RefreshToken { get; set; }
    }
}
