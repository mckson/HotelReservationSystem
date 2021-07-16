using MediatR;
using System;

namespace HotelReservation.API.Application.Commands.Room
{
    public class LockRoomCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
