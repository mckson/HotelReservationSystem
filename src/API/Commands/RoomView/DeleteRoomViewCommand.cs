using HotelReservation.API.Models.ResponseModels;
using MediatR;
using System;

namespace HotelReservation.API.Commands.RoomView
{
    public class DeleteRoomViewCommand : IRequest<RoomViewResponseModel>
    {
        public Guid Id { get; set; }
    }
}
