using HotelReservation.API.Models.ResponseModels;
using MediatR;
using System;
using System.Collections.Generic;

namespace HotelReservation.API.Commands.Reservation
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
