using HotelReservation.API.Models.ResponseModels;
using MediatR;
using System.Collections.Generic;

namespace HotelReservation.API.Application.Queries.Hotel
{
    public class GetAllHotelsNameAndIdQuery : IRequest<IEnumerable<HotelBriefResponse>>
    {
    }
}
