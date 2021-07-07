using HotelReservation.API.Models.ResponseModels;
using MediatR;
using System;

namespace HotelReservation.API.Commands.Room
{
    public class DeleteRoomCommand : IRequest<RoomResponseModel>
    {
        public Guid Id { get; set; }
    }
}
