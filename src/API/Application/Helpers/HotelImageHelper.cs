using HotelReservation.API.Application.Interfaces;
using HotelReservation.Business;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Helpers
{
    public class HotelImageHelper : IHotelImageHelper
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IHotelImageRepository _hotelImageRepository;

        public HotelImageHelper(
            IHotelImageRepository hotelImageRepository,
            IHotelRepository hotelRepository)
        {
            _hotelImageRepository = hotelImageRepository;
            _hotelRepository = hotelRepository;
        }

        public async Task ChangeHotelMainImageAsync(Guid hotelId, ImageEntity newImage)
        {
            var hotelEntity = await _hotelRepository.GetAsync(hotelId) ??
                              throw new BusinessException("Hotel does not exists", ErrorStatus.NotFound);

            var oldImage = _hotelImageRepository.Find(image => image.IsMain && image.HotelId == hotelEntity.Id).FirstOrDefault();

            if (oldImage != null && newImage != null)
            {
                oldImage.IsMain = false;
                await _hotelImageRepository.UpdateAsync(oldImage);
            }
        }
    }
}
