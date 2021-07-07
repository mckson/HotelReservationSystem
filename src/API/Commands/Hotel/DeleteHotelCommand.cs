using HotelReservation.API.Models.ResponseModels;
using MediatR;
using System;

namespace HotelReservation.API.Commands.Hotel
{
    public class DeleteHotelCommand : IRequest<HotelResponseModel>
    {
        public Guid Id { get; set; }
    }
}
