using AutoMapper;
using HotelReservation.API.Commands.Image;
using HotelReservation.Business;
using HotelReservation.Business.Interfaces;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Handlers.Image
{
    public class CreateHotelImageHandler : IRequestHandler<CreateHotelImageCommand>
    {
        private readonly IHotelImageRepository _hotelImageRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IManagementPermissionSupervisor _supervisor;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CreateHotelImageHandler(
            ILogger logger,
            IMapper mapper,
            IHotelImageRepository hotelImageRepository,
            IManagementPermissionSupervisor supervisor,
            IHotelRepository hotelRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _hotelImageRepository = hotelImageRepository;
            _supervisor = supervisor;
            _hotelRepository = hotelRepository;
        }

        public async Task<Unit> Handle(CreateHotelImageCommand request, CancellationToken cancellationToken)
        {
            _logger.Debug("Image (Hotel) is creating");

            var hotelId = Guid.Parse(request.HotelId);
            await _supervisor.CheckHotelManagementPermissionAsync(hotelId);

            var imageEntity = _mapper.Map<HotelImageEntity>(request);

            if (imageEntity.IsMain)
            {
                await ChangeHotelMainImageAsync(imageEntity.HotelId, imageEntity);
            }

            var createdImageEntity = await _hotelImageRepository.CreateAsync(imageEntity);

            _logger.Debug($"Image (Hotel) {createdImageEntity.Id} created");

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
