using System;
using System.Collections.Generic;

namespace HotelReservation.API.Models.ResponseModels
{
    public class RoomResponseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int RoomNumber { get; set; }

        public int FloorNumber { get; set; }

        public int Capacity { get; set; }

        public double Area { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public bool Smoking { get; set; }

        public bool Parking { get; set; }

        public bool IsLocked { get; set; }

        public Guid? LockedByUserId { get; set; }

        public Guid HotelId { get; set; }

        public IEnumerable<Guid> Reservations { get; set; }

        public virtual IEnumerable<RoomFacilityResponseModel> Facilities { get; set; }

        public virtual IEnumerable<RoomViewResponseModel> Views { get; set; }

        public IEnumerable<Uri> Images { get; set; }
    }
}
