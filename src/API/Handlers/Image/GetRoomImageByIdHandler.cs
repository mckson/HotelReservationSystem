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
    public class GetRoomImageByIdHandler : IRequestHandler<GetRoomImageByIdQuery, FileContentResult>
    {
        private readonly IRoomImageRepository _roomImageRepository;
        private readonly ILogger _logger;

        public GetRoomImageByIdHandler(IRoomImageRepository roomImageRepository, ILogger logger)
        {
            _roomImageRepository = roomImageRepository;
            _logger = logger;
        }

        public async Task<FileContentResult> Handle(GetRoomImageByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.Debug($"Image (Room) {request.Id} is requesting");

            var imageEntity = await _roomImageRepository.GetAsync(request.Id) ??
                              throw new BusinessException("Image with such id does not exist", ErrorStatus.NotFound);

            imageEntity.Type ??= ImageTypes.DefaultType;
            var image = new FileContentResult(imageEntity.Image, imageEntity.Type)
            {
                FileDownloadName = imageEntity.Name
            };

            _logger.Debug($"Image (Room) {request.Id} requested");

            return image;
        }
    }
}
