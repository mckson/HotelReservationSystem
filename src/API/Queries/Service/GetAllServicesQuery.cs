using HotelReservation.API.Models.ResponseModels;
using MediatR;
using System.Collections.Generic;

namespace HotelReservation.API.Queries.Service
{
    public class GetAllServicesQuery : IRequest<IEnumerable<ServiceResponseModel>>
    {
    }
}
