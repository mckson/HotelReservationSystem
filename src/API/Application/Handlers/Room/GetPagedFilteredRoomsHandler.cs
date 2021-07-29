using AutoMapper;
using HotelReservation.API.Application.Helpers;
using HotelReservation.API.Application.Interfaces;
using HotelReservation.API.Application.Queries.Room;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Filters;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Room
{
    public class GetPagedFilteredRoomsHandler : IRequestHandler<GetPagedFilteredRoomsQuery, BasePagedResponseModel<RoomResponseModel>>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        private readonly IAuthenticationHelper _authenticationHelper;

        public GetPagedFilteredRoomsHandler(
            IRoomRepository roomRepository,
            IMapper mapper,
            IAuthenticationHelper authenticationHelper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
            _authenticationHelper = authenticationHelper;
        }

        public async Task<BasePagedResponseModel<RoomResponseModel>> Handle(GetPagedFilteredRoomsQuery request, CancellationToken cancellationToken)
        {
            request.RoomsFilter.UserId = _authenticationHelper.GetCurrentUserId();

            var roomFilterExpression = FilterExpressions.GetRoomFilterExpression(request.RoomsFilter);
            var countOfFilteredRooms = await _roomRepository.GetCountAsync(roomFilterExpression);

            var validPaginationFilter = new PaginationFilter(request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize);

            var roomEntities = _roomRepository.Find(
                roomFilterExpression,
                request.PaginationFilter,
                request.RoomsFilter.PropertyName,
                request.RoomsFilter.IsDescending);

            var roomResponses = _mapper.Map<IEnumerable<RoomResponseModel>>(roomEntities);

            var pagedRoomsResponse = PaginationHelper.CreatePagedResponseModel(
                roomResponses,
                validPaginationFilter,
                countOfFilteredRooms);

            return pagedRoomsResponse;
        }
    }
}
