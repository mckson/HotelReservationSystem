using AutoMapper;
using Castle.Core.Internal;
using HotelReservation.API.Application.Helpers;
using HotelReservation.API.Application.Queries.User;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Filters;
using HotelReservation.Data.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.User
{
    public class GetPagedFilteredUsersHandler : IRequestHandler<GetPagedFilteredUsersQuery, BasePagedResponseModel<UserResponseModel>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetPagedFilteredUsersHandler(
            IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<BasePagedResponseModel<UserResponseModel>> Handle(GetPagedFilteredUsersQuery request, CancellationToken cancellationToken)
        {
            var usersFilterExpression = FilterExpressions.GetUserFilterExpression(request.UsersFilter);
            var countOfFilteredUsers = await _userRepository.GetCountAsync(usersFilterExpression);

            var validPaginationFilter =
                new PaginationFilter(request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize);

            var userEntities = await _userRepository.Find(usersFilterExpression, validPaginationFilter);

            var filteredOverRolesUserEntities = userEntities.AsEnumerable().Where(user =>
                (request.UsersFilter.Roles.IsNullOrEmpty() || request.UsersFilter.Roles.All(roleName =>
                    user.Roles.Any(role =>
                        roleName.IsNullOrEmpty() || role.StartsWith(roleName, StringComparison.InvariantCultureIgnoreCase)))));

            if (!filteredOverRolesUserEntities.Any())
            {
                throw new BusinessException(
                    "No users, that are satisfy filter parameters, were created yet",
                    ErrorStatus.NotFound);
            }

            var userResponses = _mapper.Map<IEnumerable<UserResponseModel>>(filteredOverRolesUserEntities);

            var pagedResponse = PaginationHelper.CreatePagedResponseModel(
                userResponses,
                validPaginationFilter,
                countOfFilteredUsers);

            return pagedResponse;
        }
    }
}
