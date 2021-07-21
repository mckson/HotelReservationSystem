using AutoMapper;
using Castle.Core.Internal;
using HotelReservation.API.Application.Helpers;
using HotelReservation.API.Application.Interfaces;
using HotelReservation.API.Application.Queries.Room;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Interfaces;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Filters;
using HotelReservation.Data.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Room
{
    public class GetPagedFilteredRoomsHandler : IRequestHandler<GetPagedFilteredRoomsQuery, BasePagedResponseModel<RoomResponseModel>>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;
        private readonly IAuthenticationHelper _authenticationHelper;

        public GetPagedFilteredRoomsHandler(
            IRoomRepository roomRepository,
            IMapper mapper,
            IUriService uriService,
            IAuthenticationHelper authenticationHelper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
            _uriService = uriService;
            _authenticationHelper = authenticationHelper;
        }

        public async Task<BasePagedResponseModel<RoomResponseModel>> Handle(GetPagedFilteredRoomsQuery request, CancellationToken cancellationToken)
        {
            request.RoomsFilter.UserId = _authenticationHelper.GetCurrentUserId();

            var roomFilterExpression = FilterExpressions.GetRoomFilterExpression(request.RoomsFilter);
            var countOfFilteredRooms = await _roomRepository.GetCountAsync(roomFilterExpression);

            request.PaginationFilter.PageSize ??= countOfFilteredRooms;
            var validPaginationFilter = new PaginationFilter(request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize.Value);

            var roomEntities = _roomRepository.Find(roomFilterExpression, request.PaginationFilter);

            var roomEntitiesFilteredOverFacilitiesAndRoomViews = roomEntities.AsEnumerable().Where(room =>
                (request.RoomsFilter.Facilities.IsNullOrEmpty() || request.RoomsFilter.Facilities.All(facilityName =>
                    room.Facilities.Any(facility =>
                        facilityName.IsNullOrEmpty() || facility.Name.StartsWith(facilityName, StringComparison.InvariantCultureIgnoreCase)))) &&
                (request.RoomsFilter.RoomViews.IsNullOrEmpty() || request.RoomsFilter.RoomViews.All(roomViewName =>
                    room.RoomViews.Any(roomView =>
                        roomViewName.IsNullOrEmpty() || roomView.RoomView.Name.StartsWith(roomViewName, StringComparison.InvariantCultureIgnoreCase)))));

            var roomResponses = _mapper.Map<IEnumerable<RoomResponseModel>>(roomEntitiesFilteredOverFacilitiesAndRoomViews);

            var pagedRoomsResponse = PaginationHelper.CreatePagedResponseModel(
                roomResponses,
                validPaginationFilter,
                countOfFilteredRooms,
                _uriService,
                request.Route);

            return pagedRoomsResponse;
        }
    }
}
