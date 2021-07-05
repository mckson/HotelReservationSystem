using System;
using Microsoft.AspNetCore.Identity;

namespace HotelReservation.Data.Entities
{
    public class RoleEntity : IdentityRole<Guid>
    {
        public RoleEntity()
            : base()
        {
        }

        public RoleEntity(string roleName)
            : base(roleName)
        {
        }
    }
}
