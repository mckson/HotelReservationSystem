using HotelReservation.API.Models.ResponseModels;
using MediatR;
using System;

namespace HotelReservation.API.Queries.Reservation
{
    public class GetReservationByIdQuery : IRequest<ReservationResponseModel>
    {
        public Guid Id { get; set; }
    }
}
