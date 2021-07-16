using System;
using MediatR;

namespace HotelReservation.API.Application.Commands.Image
{
    public class DeleteHotelImageCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
