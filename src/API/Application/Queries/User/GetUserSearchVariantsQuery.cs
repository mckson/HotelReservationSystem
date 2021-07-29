using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Data.Filters;
using MediatR;
using System.Collections.Generic;

namespace HotelReservation.API.Application.Queries.User
{
    public class GetUserSearchVariantsQuery : IRequest<IEnumerable<UserPromptResponseModel>>
    {
        public UsersFilter UsersFilter { get; set; }
    }
}
