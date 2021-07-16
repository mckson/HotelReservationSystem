using MediatR;
using System;

namespace HotelReservation.API.Application.Commands.Room
{
    public class UnlockRoomCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
