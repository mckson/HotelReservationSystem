using HotelReservation.API.Models.ResponseModels;
using MediatR;
using System.Collections.Generic;

namespace HotelReservation.API.Queries
{
    public class GetAllRoomsQuery : IRequest<IEnumerable<RoomResponseModel>>
    {
    }
}
