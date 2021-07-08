using HotelReservation.API.Models.ResponseModels;
using MediatR;
using System;

namespace HotelReservation.API.Commands.Reservation
{
    public class DeleteReservationCommand : IRequest<ReservationBriefResponseModel>
    {
        public Guid Id { get; set; }
    }
}
