using System;
using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
    public class RoomEntity : Entity
    {
        public string Name { get; set; }

        public int RoomNumber { get; set; }

        public int FloorNumber { get; set; }

        public int Capacity { get; set; }

        public double Area { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public bool Smoking { get; set; }

        public bool Parking { get; set; }

        public Guid? HotelId { get; set; }

        public virtual HotelEntity Hotel { get; set; }

        public virtual IEnumerable<ReservationRoomEntity> ReservationRooms { get; set; }

        public virtual List<RoomFacilityEntity> Facilities { get; set; }

        public virtual List<RoomRoomViewEntity> RoomViews { get; set; }

        public virtual ICollection<RoomImageEntity> Images { get; set; }
    }
}