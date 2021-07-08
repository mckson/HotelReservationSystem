using HotelReservation.API.Commands.Image;
using HotelReservation.Business;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Handlers.Image
{
    public class DeleteRoomImageHandler : IRequestHandler<DeleteRoomImageCommand>
    {
        private readonly IRoomImageRepository _roomImageRepository;
        private readonly ILogger _logger;

        public DeleteRoomImageHandler(IRoomImageRepository roomImageRepository, ILogger logger)
        {
            _roomImageRepository = roomImageRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteRoomImageCommand request, CancellationToken cancellationToken)
        {
            _logger.Debug($"Image (Room) {request.Id} is deleting");

            var imageEntity = await _roomImageRepository.GetAsync(request.Id) ??
                              throw new BusinessException("No image with such id", ErrorStatus.NotFound);

            // await _supervisor.CheckHotelManagementPermissionAsync(imageEntity.HotelId);
            await _roomImageRepository.DeleteAsync(imageEntity.Id);

            _logger.Debug($"Image (Room) {request.Id} deleted");

            return Unit.Value;
        }
    }
}
