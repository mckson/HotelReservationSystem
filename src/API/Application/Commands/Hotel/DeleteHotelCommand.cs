using System;
using HotelReservation.API.Models.ResponseModels;
using MediatR;

namespace HotelReservation.API.Application.Commands.Hotel
{
    public class DeleteHotelCommand : IRequest<HotelResponseModel>
    {
        public Guid Id { get; set; }
    }
}
