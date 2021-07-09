using System.Collections.Generic;
using HotelReservation.API.Models.ResponseModels;
using MediatR;

namespace HotelReservation.API.Application.Queries.Room
{
    public class GetAllRoomsQuery : IRequest<IEnumerable<RoomResponseModel>>
    {
    }
}
