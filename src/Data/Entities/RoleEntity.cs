using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace HotelReservation.Data.Entities
{
    public class RoleEntity
    {
        public int Id { get; set; }
        public string RoleName { get; set; }

        //One To Many
        public List<PermissionEntity> Permissions { get; set; }
    }
}

