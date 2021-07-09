using System.Collections.Generic;
using HotelReservation.API.Models.ResponseModels;
using MediatR;

namespace HotelReservation.API.Application.Queries.RoomView
{
    public class GetAllRoomViewsQuery : IRequest<IEnumerable<RoomViewResponseModel>>
    {
    }
}
