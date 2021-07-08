using MediatR;
using System;

namespace HotelReservation.API.Commands.Image
{
    public class DeleteHotelImageCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
