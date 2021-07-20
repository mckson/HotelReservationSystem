using MediatR;
using System;
using System.Collections.Generic;

namespace HotelReservation.API.Application.Queries.Room
{
    public class GetHotelRoomsUniqueNumbersQuery : IRequest<IEnumerable<int>>
    {
        public Guid HotelId { get; set; }
    }
}
