using HotelReservation.API.Models.ResponseModels;
using MediatR;
using System;

namespace HotelReservation.API.Queries.RoomView
{
    public class GetRoomViewByIdQuery : IRequest<RoomViewResponseModel>
    {
        public Guid Id { get; set; }
    }
}
