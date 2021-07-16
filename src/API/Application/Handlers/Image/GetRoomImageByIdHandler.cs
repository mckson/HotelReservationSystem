using HotelReservation.API.Application.Queries.Image;
using HotelReservation.Business;
using HotelReservation.Business.Constants;
using HotelReservation.Data.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Image
{
    public class GetRoomImageByIdHandler : IRequestHandler<GetRoomImageByIdQuery, FileContentResult>
    {
        private readonly IRoomImageRepository _roomImageRepository;

        public GetRoomImageByIdHandler(IRoomImageRepository roomImageRepository)
        {
            _roomImageRepository = roomImageRepository;
        }

        public async Task<FileContentResult> Handle(GetRoomImageByIdQuery request, CancellationToken cancellationToken)
        {
            var imageEntity = await _roomImageRepository.GetAsync(request.Id) ??
                              throw new BusinessException("Image with such id does not exist", ErrorStatus.NotFound);

            imageEntity.Type ??= ImageTypes.DefaultType;
            var image = new FileContentResult(imageEntity.Image, imageEntity.Type)
            {
                FileDownloadName = imageEntity.Name
            };

            return image;
        }
    }
}
