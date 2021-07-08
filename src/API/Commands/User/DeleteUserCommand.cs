using MediatR;
using System;

namespace HotelReservation.API.Commands.User
{
    public class DeleteUserCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
