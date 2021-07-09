using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotelReservation.API.Application.Commands.Image;
using HotelReservation.Business;
using HotelReservation.Business.Interfaces;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;

namespace HotelReservation.API.Application.Handlers.Image
{
    public class UpdateHotelImageToMainHandler : IRequestHandler<UpdateHotelImageToMainCommand>
    {
        private readonly IHotelImageRepository _hotelImageRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly ILogger _logger;
        private readonly IManagementPermissionSupervisor _supervisor;

        public UpdateHotelImageToMainHandler(IHotelImageRepository hotelImageRepository, ILogger logger, IManagementPermissionSupervisor supervisor, IHotelRepository hotelRepository)
        {
            _hotelImageRepository = hotelImageRepository;
            _logger = logger;
            _supervisor = supervisor;
            _hotelRepository = hotelRepository;
        }

        public async Task<Unit> Handle(UpdateHotelImageToMainCommand request, CancellationToken cancellationToken)
        {
            _logger.Debug($"Image (Hotel) {request.Id} is updating");

            var imageEntity = await _hotelImageRepository.GetAsync(request.Id) ??
                              throw new BusinessException("No image with such id", ErrorStatus.NotFound);

            await _supervisor.CheckHotelManagementPermissionAsync(imageEntity.HotelId);
            await ChangeHotelMainImageAsync(imageEntity.HotelId, imageEntity);
            imageEntity.IsMain = true;

            await _hotelImageRepository.UpdateAsync(imageEntity);

            _logger.Debug($"Image (Hotel) {request.Id} is updated");

            return Unit.Value;
        }

        private async Task ChangeHotelMainImageAsync(Guid hotelId, ImageEntity newImage)
        {
            var hotelEntity = await _hotelRepository.GetAsync(hotelId) ??
                              throw new BusinessException("Hotel does not exists", ErrorStatus.NotFound);

            _logger.Debug($"Main image of hotel {hotelEntity.Id} is updating");

            var oldImage = _hotelImageRepository.Find(image => image.IsMain && image.HotelId == hotelEntity.Id).FirstOrDefault();

            if (oldImage != null && newImage != null)
            {
                oldImage.IsMain = false;
                await _hotelImageRepository.UpdateAsync(oldImage);
            }

            _logger.Debug($"Main image of hotel {hotelEntity.Id} is updated");
        }
    }
}
