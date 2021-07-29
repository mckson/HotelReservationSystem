using AutoMapper;
using HotelReservation.API.Application.Queries.Room;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Constants;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Filters;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Room
{
    public class GetRoomSearchVariantsHandler : IRequestHandler<GetRoomSearchVariantsQuery, IEnumerable<RoomPromptResponseModel>>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public GetRoomSearchVariantsHandler(
            IRoomRepository roomRepository,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoomPromptResponseModel>> Handle(GetRoomSearchVariantsQuery request, CancellationToken cancellationToken)
        {
            var roomFilterExpression = FilterExpressions.GetRoomFilterExpression(request.RoomsFilter);
            var paginationFilter = new PaginationFilter(
                PaginationValuesForSearchVariants.PageNumber,
                PaginationValuesForSearchVariants.PageSize);

            var searchVariants = await Task.FromResult(_roomRepository.Find(roomFilterExpression, paginationFilter));

            var searchVariantsResponses = _mapper.Map<IEnumerable<RoomPromptResponseModel>>(searchVariants);

            return searchVariantsResponses;
        }
    }
}
