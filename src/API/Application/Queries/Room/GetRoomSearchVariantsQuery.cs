using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Data.Filters;
using MediatR;
using System.Collections.Generic;

namespace HotelReservation.API.Application.Queries.Room
{
    public class GetRoomSearchVariantsQuery : IRequest<IEnumerable<RoomPromptResponseModel>>
    {
        public RoomsFilter RoomsFilter { get; set; }
    }
}
