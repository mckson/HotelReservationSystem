using System;
using HotelReservation.API.Models.ResponseModels;
using MediatR;

namespace HotelReservation.API.Application.Commands.Room
{
    public class DeleteRoomCommand : IRequest<RoomResponseModel>
    {
        public Guid Id { get; set; }
    }
}
