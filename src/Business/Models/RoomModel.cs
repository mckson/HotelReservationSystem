﻿using System.Collections.Generic;

namespace HotelReservation.Business.Models
{
    public class RoomModel
    {
        public int Id { get; set; }

        public int RoomNumber { get; set; }

        public int FloorNumber { get; set; }

        public int Capacity { get; set; }

        public double Price { get; set; }

        public int HotelId { get; set; }

        public HotelModel Hotel { get; set; }

        public IEnumerable<ReservationRoomModel> ReservationRooms { get; set; }
    }
}
