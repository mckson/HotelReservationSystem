using MediatR;
using System;
using System.Collections.Generic;

namespace HotelReservation.API.Application.Queries.Room
{
    public class GetHotelRoomsUniqueNamesQuery : IRequest<IEnumerable<string>>
    {
        public Guid HotelId { get; set; }
    }
}
