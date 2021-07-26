using AutoMapper;
using HotelReservation.API.Application.Queries.RoomView;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Constants;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Filters;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.RoomView
{
    public class GetRoomViewSearchVariantsHandler : IRequestHandler<GetRoomViewSearchVariantsQuery, IEnumerable<RoomViewFilterResponseModel>>
    {
        private readonly IRoomViewRepository _roomViewRepository;
        private readonly IMapper _mapper;

        public GetRoomViewSearchVariantsHandler(
            IRoomViewRepository roomViewRepository,
            IMapper mapper)
        {
            _roomViewRepository = roomViewRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoomViewFilterResponseModel>> Handle(GetRoomViewSearchVariantsQuery request, CancellationToken cancellationToken)
        {
            var paginationFilter = new PaginationFilter(
                PaginationValuesForSearchVariants.PageNumber,
                PaginationValuesForSearchVariants.PageSize);

            var roomViewsFilterExpression =
                FilterExpressions.GetRoomViewEntityFilterExpression(request.RoomViewsFilter);

            var searchVariants =
                await Task.FromResult(_roomViewRepository.Find(roomViewsFilterExpression, paginationFilter));

            var searchVariantsResponses = _mapper.Map<IEnumerable<RoomViewFilterResponseModel>>(searchVariants);

            return searchVariantsResponses;
        }
    }
}
