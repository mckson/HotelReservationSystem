using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Data.Filters;
using MediatR;
using System.Collections.Generic;

namespace HotelReservation.API.Application.Queries.Hotel
{
    public class GetHotelSearchVariantsQuery : IRequest<IEnumerable<HotelFilterResponseModel>>
    {
        public HotelsFilter HotelsFilter { get; set; }
    }
}
