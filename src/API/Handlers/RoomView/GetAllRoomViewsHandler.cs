using AutoMapper;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.API.Queries.RoomView;
using HotelReservation.Business;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Handlers.RoomView
{
    public class GetAllRoomViewsHandler : IRequestHandler<GetAllRoomViewsQuery, IEnumerable<RoomViewResponseModel>>
    {
        private readonly IRoomViewRepository _roomViewRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetAllRoomViewsHandler(IRoomViewRepository roomViewRepository, ILogger logger, IMapper mapper)
        {
            _roomViewRepository = roomViewRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoomViewResponseModel>> Handle(GetAllRoomViewsQuery request, CancellationToken cancellationToken)
        {
            _logger.Debug("Room views are requesting");

            var roomViewEntities = await Task.FromResult(_roomViewRepository.GetAll()) ??
                                   throw new BusinessException("No room views were created", ErrorStatus.NotFound);

            var roomViewResponses = _mapper.Map<IEnumerable<RoomViewResponseModel>>(roomViewEntities);

            _logger.Debug("Room views are requested");

            return roomViewResponses;
        }
    }
}
