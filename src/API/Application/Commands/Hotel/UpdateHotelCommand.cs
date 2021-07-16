﻿using System;
using System.Collections.Generic;
using HotelReservation.API.Models.RequestModels;
using HotelReservation.API.Models.ResponseModels;
using MediatR;

namespace HotelReservation.API.Application.Commands.Hotel
{
    public class UpdateHotelCommand : IRequest<HotelResponseModel>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int NumberFloors { get; set; }

        public double Deposit { get; set; }

        public string Description { get; set; }

        public LocationRequestModel Location { get; set; }

        public IEnumerable<Guid> Managers { get; set; }
    }
}
