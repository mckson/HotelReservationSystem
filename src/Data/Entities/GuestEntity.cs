﻿using System;

namespace HotelReservation.Data.Entities
{
    public class GuestEntity /*: Person*/
    {
        public int Id { get; set; }
        
        public UserEntity User { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public int RoomId { get; set; }
        //Related entity
        public RoomEntity Room { get; set; }

        public int HotelId { get; set; }
        public HotelEntity Hotel { get; set; }

        public ReservationEntity Reservation { get; set; }
    }
}
