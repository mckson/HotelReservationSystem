using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HotelReservation.API.Commands.Room;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Business.Interfaces;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;

namespace HotelReservation.API.Handlers.Room
{
    public class CreateRoomHandler : IRequestHandler<CreateRoomCommand, RoomResponseModel>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IManagementPermissionSupervisor _supervisor;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CreateRoomHandler(
            IRoomRepository roomRepository,
            IMapper mapper,
            ILogger logger,
            IHotelRepository hotelRepository,
            IManagementPermissionSupervisor supervisor)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
            _logger = logger;
            _hotelRepository = hotelRepository;
            _supervisor = supervisor;
        }

        public async Task<RoomResponseModel> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            _logger.Debug("Room is creating");

            var roomEntity = _mapper.Map<RoomEntity>(request);

            if (roomEntity.HotelId != null)
            {
                var hotelEntity = await _hotelRepository.GetAsync(roomEntity.HotelId.Value) ??
                                  throw new BusinessException("No hotel with such id", ErrorStatus.NotFound);

                await _supervisor.CheckHotelManagementPermissionAsync(hotelEntity.Id);

                if (hotelEntity.Rooms.Any(room => room.RoomNumber == roomEntity.RoomNumber))
                    throw new BusinessException("Hotel already has room with such number", ErrorStatus.AlreadyExist);

                if (request.FloorNumber > hotelEntity.NumberFloors)
                    throw new BusinessException($"There are only {hotelEntity.NumberFloors} floors in {hotelEntity.Name}", ErrorStatus.IncorrectInput);
            }
            else
            {
                throw new BusinessException(
                    "Hotel id was null. Room cannot be created without hotel",
                    ErrorStatus.NotFound);
            }

            var createdRoomEntity = await _roomRepository.CreateAsync(roomEntity);
            var createdRoomResponse = _mapper.Map<RoomResponseModel>(createdRoomEntity);

            _logger.Debug($"Room {createdRoomResponse.Id} created");

            return createdRoomResponse;
        }
    }
}
