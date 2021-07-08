using HotelReservation.API.Models.ResponseModels;
using MediatR;
using System.Collections.Generic;

namespace HotelReservation.API.Queries.User
{
    public class GetAllUsersQuery : IRequest<IEnumerable<UserBriefResponseModel>>
    {
    }
}
