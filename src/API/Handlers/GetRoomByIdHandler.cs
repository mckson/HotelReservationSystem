using AutoMapper;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.API.Queries;
using HotelReservation.Business;
using HotelReservation.Business.Models;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Handlers
{
    public class GetRoomByIdHandler : IRequestHandler<GetRoomByIdQuery, RoomResponseModel>, IRequest<RoomResponseModel>
    {
        private readonly IMapper _mapper;
        private readonly IRoomRepository _roomRepository;
        private readonly ILogger _logger;

        public GetRoomByIdHandler(IRoomRepository roomRepository, IMapper mapper, ILogger logger)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RoomResponseModel> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.Debug($"Room {request.Id} is requesting");

            var roomEntity = await _roomRepository.GetAsync(request.Id) ??
                             throw new BusinessException("No room with such id", ErrorStatus.NotFound);

            var roomModel = _mapper.Map<RoomModel>(roomEntity);
            var roomResponse = _mapper.Map<RoomResponseModel>(roomModel);

            _logger.Debug($"Room {request.Id} requested");

            return roomResponse;
        }
    }
}
