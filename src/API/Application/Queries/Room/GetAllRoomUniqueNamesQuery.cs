using MediatR;
using System.Collections.Generic;

namespace HotelReservation.API.Application.Queries.Room
{
    public class GetAllRoomUniqueNamesQuery : IRequest<IEnumerable<string>>
    {
    }
}
