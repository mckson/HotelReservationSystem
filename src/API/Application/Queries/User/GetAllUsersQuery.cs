using System.Collections.Generic;
using HotelReservation.API.Models.ResponseModels;
using MediatR;

namespace HotelReservation.API.Application.Queries.User
{
    public class GetAllUsersQuery : IRequest<IEnumerable<UserBriefResponseModel>>
    {
    }
}
