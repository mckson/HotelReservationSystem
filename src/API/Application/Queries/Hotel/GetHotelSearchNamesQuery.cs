using MediatR;
using System.Collections.Generic;

namespace HotelReservation.API.Application.Queries.Hotel
{
    public class GetHotelSearchNamesQuery : IRequest<IEnumerable<string>>
    {
        public string SearchName { get; set; }
    }
}
