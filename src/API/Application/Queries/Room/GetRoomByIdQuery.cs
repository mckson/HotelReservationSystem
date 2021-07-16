using System;
using HotelReservation.API.Models.ResponseModels;
using MediatR;

namespace HotelReservation.API.Application.Queries.Room
{
    public class GetRoomByIdQuery : IRequest<RoomResponseModel>
    {
        public GetRoomByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
