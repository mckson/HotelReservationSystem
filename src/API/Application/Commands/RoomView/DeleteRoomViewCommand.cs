using System;
using HotelReservation.API.Models.ResponseModels;
using MediatR;

namespace HotelReservation.API.Application.Commands.RoomView
{
    public class DeleteRoomViewCommand : IRequest<RoomViewResponseModel>
    {
        public Guid Id { get; set; }
    }
}
