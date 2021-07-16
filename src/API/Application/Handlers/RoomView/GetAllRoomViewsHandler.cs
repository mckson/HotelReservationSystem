using AutoMapper;
using HotelReservation.API.Application.Queries.RoomView;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.RoomView
{
    public class GetAllRoomViewsHandler : IRequestHandler<GetAllRoomViewsQuery, IEnumerable<RoomViewResponseModel>>
    {
        private readonly IRoomViewRepository _roomViewRepository;
        private readonly IMapper _mapper;

        public GetAllRoomViewsHandler(
            IRoomViewRepository roomViewRepository,
            IMapper mapper)
        {
            _roomViewRepository = roomViewRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoomViewResponseModel>> Handle(GetAllRoomViewsQuery request, CancellationToken cancellationToken)
        {
            var roomViewEntities = await Task.FromResult(_roomViewRepository.GetAll()) ??
                                   throw new BusinessException("No room views were created", ErrorStatus.NotFound);

            var roomViewResponses = _mapper.Map<IEnumerable<RoomViewResponseModel>>(roomViewEntities);

            return roomViewResponses;
        }
    }
}
