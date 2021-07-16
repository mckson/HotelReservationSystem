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
    public class GetHotelImageByIdHandler : IRequestHandler<GetHotelImageByIdQuery, FileContentResult>
    {
        private readonly IHotelImageRepository _hotelImageRepository;

        public GetHotelImageByIdHandler(IHotelImageRepository hotelImageRepository)
        {
            _hotelImageRepository = hotelImageRepository;
        }

        public async Task<FileContentResult> Handle(GetHotelImageByIdQuery request, CancellationToken cancellationToken)
        {
            var imageEntity = await _hotelImageRepository.GetAsync(request.Id) ??
                              throw new BusinessException("Image with such id does not exist", ErrorStatus.NotFound);

            imageEntity.Type ??= ImageTypes.DefaultType;

            var image = new FileContentResult(imageEntity.Image, imageEntity.Type) { FileDownloadName = imageEntity.Name };

            return image;
        }
    }
}
