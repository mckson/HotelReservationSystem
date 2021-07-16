using System;
using System.Collections.Generic;
using HotelReservation.API.Models.ResponseModels;
using MediatR;

namespace HotelReservation.API.Application.Commands.Reservation
{
    public class CreateReservationCommand : IRequest<ReservationBriefResponseModel>
    {
        public string HotelId { get; set; }

        public IEnumerable<string> Rooms { get; set; }

        public IEnumerable<string> Services { get; set; }

        public DateTime DateIn { get; set; }

        public DateTime DateOut { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}
