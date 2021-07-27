using AutoMapper;
using HotelReservation.API.Application.Queries.User;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Constants;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Filters;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.User
{
    public class GetUserSearchVariantsHandler : IRequestHandler<GetUserSearchVariantsQuery, IEnumerable<UserPromptResponseModel>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserSearchVariantsHandler(
            IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserPromptResponseModel>> Handle(GetUserSearchVariantsQuery request, CancellationToken cancellationToken)
        {
            var userFilterExpression = FilterExpressions.GetUserFilterExpression(request.UsersFilter);
            var paginationFilter = new PaginationFilter(
                PaginationValuesForSearchVariants.PageNumber,
                PaginationValuesForSearchVariants.PageSize);

            var searchVariants = await _userRepository.Find(userFilterExpression, paginationFilter);

            var searchVariantsResponses = _mapper.Map<IEnumerable<UserPromptResponseModel>>(searchVariants);

            return searchVariantsResponses;
        }
    }
}
