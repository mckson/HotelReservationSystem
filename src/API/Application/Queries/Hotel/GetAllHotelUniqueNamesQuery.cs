using MediatR;
using System.Collections.Generic;

namespace HotelReservation.API.Application.Queries.Hotel
{
    public class GetAllHotelUniqueNamesQuery : IRequest<IEnumerable<string>>
    {
    }
}
