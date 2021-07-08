using HotelReservation.API.Models.ResponseModels;
using MediatR;
using System.Collections.Generic;

namespace HotelReservation.API.Queries.RoomView
{
    public class GetAllRoomViewsQuery : IRequest<IEnumerable<RoomViewResponseModel>>
    {
    }
}
