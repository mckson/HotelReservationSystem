﻿using HotelReservation.API.Models.ResponseModels;
using MediatR;
using System;
using System.Collections.Generic;

namespace HotelReservation.API.Commands.Room
{
    public class CreateRoomCommand : IRequest<RoomResponseModel>
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

        public string HotelId { get; set; }

        public IEnumerable<string> Facilities { get; set; }

        public IEnumerable<Guid> Views { get; set; }
    }
}
