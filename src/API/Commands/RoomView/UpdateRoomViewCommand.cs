using HotelReservation.API.Models.ResponseModels;
using MediatR;
using System;

namespace HotelReservation.API.Commands.RoomView
{
    public class UpdateRoomViewCommand : IRequest<RoomViewResponseModel>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
