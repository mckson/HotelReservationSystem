using System;
using HotelReservation.API.Models.ResponseModels;
using MediatR;

namespace HotelReservation.API.Application.Queries.Reservation
{
    public class GetReservationByIdQuery : IRequest<ReservationResponseModel>
    {
        public Guid Id { get; set; }
    }
}
