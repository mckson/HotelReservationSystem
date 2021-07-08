using HotelReservation.API.Commands.Image;
using HotelReservation.Business;
using HotelReservation.Business.Interfaces;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Handlers.Image
{
    public class DeleteHotelImageHandler : IRequestHandler<DeleteHotelImageCommand>
    {
        private readonly IHotelImageRepository _hotelImageRepository;
        private readonly IManagementPermissionSupervisor _supervisor;
        private readonly ILogger _logger;

        public DeleteHotelImageHandler(IHotelImageRepository hotelImageRepository, ILogger logger, IManagementPermissionSupervisor supervisor)
        {
            _hotelImageRepository = hotelImageRepository;
            _logger = logger;
            _supervisor = supervisor;
        }

        public async Task<Unit> Handle(DeleteHotelImageCommand request, CancellationToken cancellationToken)
        {
            _logger.Debug($"Image (Hotel) {request.Id} is deleting");

            var imageEntity = await _hotelImageRepository.GetAsync(request.Id) ??
                              throw new BusinessException("No image with such id", ErrorStatus.NotFound);

            await _supervisor.CheckHotelManagementPermissionAsync(imageEntity.HotelId);
            await _hotelImageRepository.DeleteAsync(request.Id);

            _logger.Debug($"Image (Hotel) {request.Id} deleted");

            return Unit.Value;
        }
    }
}
