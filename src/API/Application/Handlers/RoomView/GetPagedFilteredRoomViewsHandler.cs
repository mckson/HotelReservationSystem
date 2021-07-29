using AutoMapper;
using HotelReservation.API.Application.Helpers;
using HotelReservation.API.Application.Queries.RoomView;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Filters;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.RoomView
{
    public class GetPagedFilteredRoomViewsHandler : IRequestHandler<GetPagedFilteredRoomViewsQuery, BasePagedResponseModel<RoomViewResponseModel>>
    {
        private readonly IRoomViewRepository _roomViewRepository;
        private readonly IMapper _mapper;

        public GetPagedFilteredRoomViewsHandler(
            IRoomViewRepository roomViewRepository,
            IMapper mapper)
        {
            _roomViewRepository = roomViewRepository;
            _mapper = mapper;
        }

        public async Task<BasePagedResponseModel<RoomViewResponseModel>> Handle(GetPagedFilteredRoomViewsQuery request, CancellationToken cancellationToken)
        {
            var roomViewFilterExpression = FilterExpressions.GetRoomViewEntityFilterExpression(request.RoomViewsFilter);
            var countOfFilteredRoomViews = await _roomViewRepository.GetCountAsync(roomViewFilterExpression);
            var validPaginationFilter =
                new PaginationFilter(request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize);

            var roomViewEntities = _roomViewRepository.Find(
                roomViewFilterExpression,
                validPaginationFilter,
                request.RoomViewsFilter.PropertyName,
                request.RoomViewsFilter.IsDescending);

            var roomViewResponses = _mapper.Map<IEnumerable<RoomViewResponseModel>>(roomViewEntities);

            var pagedResponse = PaginationHelper.CreatePagedResponseModel(
                roomViewResponses,
                validPaginationFilter,
                countOfFilteredRoomViews);

            return pagedResponse;
        }
    }
}
