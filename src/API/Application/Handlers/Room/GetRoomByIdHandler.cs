using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HotelReservation.API.Application.Queries.Room;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;

namespace HotelReservation.API.Application.Handlers.Room
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

            var roomResponse = _mapper.Map<RoomResponseModel>(roomEntity);

            _logger.Debug($"Room {request.Id} requested");

            return roomResponse;
        }
    }
}
