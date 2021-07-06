using HotelReservation.API.Models.ResponseModels;
using MediatR;
using System;

namespace HotelReservation.API.Queries
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
