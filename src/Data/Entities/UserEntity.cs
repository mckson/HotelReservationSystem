﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace HotelReservation.Data.Entities
{
    public class UserEntity : IdentityUser<Guid>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int? RoomId { get; set; }

        public virtual RoomEntity Room { get; set; }

        // public int? HotelId { get; set; }
        // public virtual HotelEntity Hotel { get; set; }
        public virtual List<HotelUserEntity> HotelUsers { get; set; }

        // public virtual IEnumerable<ReservationEntity> Reservations { get; set; }
        public virtual RefreshTokenEntity RefreshToken { get; set; }
    }
}
