using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Data.Filters;
using MediatR;

namespace HotelReservation.API.Application.Queries.User
{
    public class GetPagedFilteredUsersQuery : IRequest<BasePagedResponseModel<UserResponseModel>>
    {
        public PaginationFilter PaginationFilter { get; set; }

        public UsersFilter UsersFilter { get; set; }

        public string Route { get; set; }
    }
}
