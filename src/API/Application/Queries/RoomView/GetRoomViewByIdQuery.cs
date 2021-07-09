using System;
using HotelReservation.API.Models.ResponseModels;
using MediatR;

namespace HotelReservation.API.Application.Queries.RoomView
{
    public class GetRoomViewByIdQuery : IRequest<RoomViewResponseModel>
    {
        public Guid Id { get; set; }
    }
}
