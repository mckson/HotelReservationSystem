using HotelReservation.API.Models.ResponseModels;
using MediatR;
using System;

namespace HotelReservation.API.Queries.Hotel
{
    public class GetHotelByIdQuery : IRequest<HotelResponseModel>
    {
        public Guid Id { get; set; }
    }
}
