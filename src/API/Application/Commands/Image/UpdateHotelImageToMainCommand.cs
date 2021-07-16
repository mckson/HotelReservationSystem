using System;
using MediatR;

namespace HotelReservation.API.Application.Commands.Image
{
    public class UpdateHotelImageToMainCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
