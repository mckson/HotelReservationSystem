using HotelReservation.API.Models.RequestModels;
using HotelReservation.API.Models.ResponseModels;
using MediatR;
using System;
using System.Collections.Generic;

namespace HotelReservation.API.Commands.Hotel
{
    public class CreateHotelCommand : IRequest<HotelResponseModel>
    {
        public string Name { get; set; }

        public int NumberFloors { get; set; }

        public double Deposit { get; set; }

        public string Description { get; set; }

        public LocationRequestModel Location { get; set; }

        public IEnumerable<Guid> Managers { get; set; }
    }
}
