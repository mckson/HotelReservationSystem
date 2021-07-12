using AutoMapper;
using HotelReservation.API.Application.Helpers;
using HotelReservation.API.Application.Queries.User;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Business.Interfaces;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Filters;
using HotelReservation.Data.Interfaces;
using MediatR;
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
        private readonly IUriService _uriService;

        public GetPagedFilteredUsersHandler(
            IUserRepository userRepository,
            IMapper mapper,
            IUriService uriService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _uriService = uriService;
        }

        public async Task<BasePagedResponseModel<UserResponseModel>> Handle(GetPagedFilteredUsersQuery request, CancellationToken cancellationToken)
        {
            var usersFilterExpression = FilterExpressions.GetUserFilterExpression(request.UsersFilter);
            var countOfFilteredUsers = await _userRepository.GetCountAsync(usersFilterExpression);

            request.PaginationFilter.PageSize ??= countOfFilteredUsers;
            var validPaginationFilter =
                new PaginationFilter(request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize.Value);

            var userEntities = await _userRepository.Find(usersFilterExpression, validPaginationFilter);

            if (!userEntities.Any())
            {
                throw new BusinessException(
                    "No users, that are satisfy filter parameters, were created yet",
                    ErrorStatus.NotFound);
            }

            var userResponses = _mapper.Map<IEnumerable<UserResponseModel>>(userEntities);

            var pagedResponse = PaginationHelper.CreatePagedResponseModel(
                userResponses,
                validPaginationFilter,
                countOfFilteredUsers,
                _uriService,
                request.Route);

            return pagedResponse;
        }
    }
}
