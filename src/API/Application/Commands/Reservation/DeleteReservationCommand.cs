using System;
using HotelReservation.API.Models.ResponseModels;
using MediatR;

namespace HotelReservation.API.Application.Commands.Reservation
{
    public class DeleteReservationCommand : IRequest<ReservationBriefResponseModel>
    {
        public Guid Id { get; set; }
    }
}
