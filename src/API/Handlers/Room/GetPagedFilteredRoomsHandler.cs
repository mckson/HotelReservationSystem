using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HotelReservation.API.Helpers;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.API.Queries.Room;
using HotelReservation.Business.Interfaces;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Filters;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;

namespace HotelReservation.API.Handlers.Room
{
    public class GetPagedFilteredRoomsHandler : IRequestHandler<GetPagedFilteredRoomsQuery, BasePagedResponseModel<RoomResponseModel>>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public GetPagedFilteredRoomsHandler(IRoomRepository roomRepository, IMapper mapper, ILogger logger, IUriService uriService)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
            _logger = logger;
            _uriService = uriService;
        }

        public async Task<BasePagedResponseModel<RoomResponseModel>> Handle(GetPagedFilteredRoomsQuery request, CancellationToken cancellationToken)
        {
            _logger.Debug($"Paged rooms are requesting, page: {request.PaginationFilter.PageNumber}, size: {request.PaginationFilter.PageSize}");

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

            _logger.Debug($"Paged rooms are requested, page: {request.PaginationFilter.PageNumber}, size: {request.PaginationFilter.PageSize}");

            return pagedRoomsResponse;
        }
    }
}
