using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HotelReservation.API.Application.Queries.RoomView;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;

namespace HotelReservation.API.Application.Handlers.RoomView
{
    public class GetRoomViewByIdHandler : IRequestHandler<GetRoomViewByIdQuery, RoomViewResponseModel>
    {
        private readonly IRoomViewRepository _roomViewRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetRoomViewByIdHandler(IRoomViewRepository roomViewRepository, ILogger logger, IMapper mapper)
        {
            _roomViewRepository = roomViewRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<RoomViewResponseModel> Handle(GetRoomViewByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.Debug($"Room view {request.Id} is requesting");

            var roomViewEntity = await _roomViewRepository.GetAsync(request.Id) ?? throw new BusinessException(
                "Room view with such id does not exist",
                ErrorStatus.NotFound);

            var roomViewResponse = _mapper.Map<RoomViewResponseModel>(roomViewEntity);

            _logger.Debug($"Room view {request.Id} is requested");

            return roomViewResponse;
        }
    }
}
