using System;
using MediatR;

namespace HotelReservation.API.Application.Commands.User
{
    public class DeleteUserCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
