using MediatR;
using System;

namespace HotelReservation.API.Commands.Image
{
    public class DeleteRoomImageCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
