﻿using System;
using System.Collections.Generic;

namespace HotelReservation.Business.Models
{
    public class RoomModel
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

        public Guid HotelId { get; set; }

        public HotelModel Hotel { get; set; }

        public IEnumerable<ReservationRoomModel> ReservationRooms { get; set; }

        public virtual IEnumerable<RoomFacilityModel> Facilities { get; set; }

        public virtual IEnumerable<RoomRoomViewModel> RoomViews { get; set; }

        public virtual ICollection<RoomImageModel> Images { get; set; }
    }
}
