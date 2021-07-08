using MediatR;
using System;

namespace HotelReservation.API.Commands.Image
{
    public class UpdateHotelImageToMainCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
