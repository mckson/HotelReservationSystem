using System;
using MediatR;

namespace HotelReservation.API.Application.Commands.Image
{
    public class DeleteRoomImageCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
