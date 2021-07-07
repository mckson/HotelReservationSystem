using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HotelReservation.API.Commands.Room;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Business.Interfaces;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;

namespace HotelReservation.API.Handlers.Room
{
    public class DeleteRoomHandler : IRequestHandler<DeleteRoomCommand, RoomResponseModel>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IManagementPermissionSupervisor _supervisor;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public DeleteRoomHandler(
            IRoomRepository roomRepository,
            IMapper mapper,
            ILogger logger,
            IManagementPermissionSupervisor supervisor)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
            _logger = logger;
            _supervisor = supervisor;
        }

        public async Task<RoomResponseModel> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
        {
            _logger.Debug($"Room {request.Id} is deleting");

            // was as no tracking
            var roomEntity = await _roomRepository.GetAsync(request.Id) ??
                             throw new BusinessException("No room with such id", ErrorStatus.NotFound);

            if (roomEntity.HotelId != null)
            {
                await _supervisor.CheckHotelManagementPermissionAsync(roomEntity.HotelId.Value);
            }
            else
            {
                throw new BusinessException(
                    "Hotel id was null. Room cannot be created without hotel",
                    ErrorStatus.NotFound);
            }

            var deletedRoomEntity = await _roomRepository.DeleteAsync(request.Id);
            var deletedRoomResponse = _mapper.Map<RoomResponseModel>(deletedRoomEntity);

            _logger.Debug($"Room {deletedRoomResponse.Id} deleted");

            return deletedRoomResponse;
        }
    }
}
