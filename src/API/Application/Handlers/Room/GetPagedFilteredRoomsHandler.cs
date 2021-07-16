using AutoMapper;
using HotelReservation.API.Application.Helpers;
using HotelReservation.API.Application.Queries.Room;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Interfaces;
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
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;

        public GetPagedFilteredRoomsHandler(
            IRoomRepository roomRepository,
            IMapper mapper,
            IUriService uriService)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
            _uriService = uriService;
        }

        public async Task<BasePagedResponseModel<RoomResponseModel>> Handle(GetPagedFilteredRoomsQuery request, CancellationToken cancellationToken)
        {
            var roomFilterExpression = FilterExpressions.GetRoomFilterExpression(request.RoomsFilter);
            var countOfFilteredRooms = await _roomRepository.GetCountAsync(roomFilterExpression);

            request.PaginationFilter.PageSize ??= countOfFilteredRooms;
            var validPaginationFilter = new PaginationFilter(request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize.Value);

            var roomEntities = _roomRepository.Find(roomFilterExpression, request.PaginationFilter);
            var roomResponses = _mapper.Map<IEnumerable<RoomResponseModel>>(roomEntities);

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
