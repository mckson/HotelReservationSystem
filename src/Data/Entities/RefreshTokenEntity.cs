using System;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Entities
{
    [Owned]
    public class RefreshTokenEntity : Entity
    {
        public string Token { get; set; }

        public DateTime Expires { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Revoked { get; set; }

        public string ReplacedByToken { get; set; }

        public bool IsExpired => DateTime.UtcNow >= Expires;

        public bool IsActive => Revoked == null && !IsExpired;
    }
}
