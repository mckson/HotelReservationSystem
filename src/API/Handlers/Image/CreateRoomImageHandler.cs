using AutoMapper;
using HotelReservation.API.Commands.Image;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Handlers.Image
{
    public class CreateRoomImageHandler : IRequestHandler<CreateRoomImageCommand>
    {
        private readonly IRoomImageRepository _roomImageRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CreateRoomImageHandler(IRoomImageRepository roomImageRepository, IMapper mapper, ILogger logger)
        {
            _roomImageRepository = roomImageRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateRoomImageCommand request, CancellationToken cancellationToken)
        {
            _logger.Debug("Image (Room) is creating");

            // await _supervisor.CheckHotelManagementPermissionAsync(imageModel.HotelId);
            var imageEntity = _mapper.Map<RoomImageEntity>(request);

            var createdImageEntity = await _roomImageRepository.CreateAsync(imageEntity);

            _logger.Debug($"Image (Room) {createdImageEntity.Id} created");

            return Unit.Value;
        }
    }
}
