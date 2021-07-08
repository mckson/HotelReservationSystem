using HotelReservation.API.Queries.Image;
using HotelReservation.Business;
using HotelReservation.Business.Constants;
using HotelReservation.Data.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Handlers.Image
{
    public class GetHotelImageByIdHandler : IRequestHandler<GetHotelImageByIdQuery, FileContentResult>
    {
        private readonly IHotelImageRepository _hotelImageRepository;
        private readonly ILogger _logger;

        public GetHotelImageByIdHandler(IHotelImageRepository hotelImageRepository, ILogger logger)
        {
            _hotelImageRepository = hotelImageRepository;
            _logger = logger;
        }

        public async Task<FileContentResult> Handle(GetHotelImageByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.Debug($"Image (Hotel) {request.Id} is requesting");

            var imageEntity = await _hotelImageRepository.GetAsync(request.Id) ??
                              throw new BusinessException("Image with such id does not exist", ErrorStatus.NotFound);

            imageEntity.Type ??= ImageTypes.DefaultType;

            var image = new FileContentResult(imageEntity.Image, imageEntity.Type) { FileDownloadName = imageEntity.Name };

            _logger.Debug($"Image (Hotel) {request.Id} requested");

            return image;
        }
    }
}
