using System;
using HotelReservation.API.Models.ResponseModels;
using MediatR;

namespace HotelReservation.API.Application.Queries.Hotel
{
    public class GetHotelByIdQuery : IRequest<HotelResponseModel>
    {
        public Guid Id { get; set; }
    }
}
