using AutoMapper;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservation.Business.Services
{
    public class HotelImagesService : IHotelImagesService
    {
        private readonly IHotelImageRepository _hotelImageRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IHotelRepository _hotelRepository;
        private readonly IManagementPermissionSupervisor _supervisor;

        public HotelImagesService(
            IHotelImageRepository hotelImageRepository,
            IHotelRepository hotelRepository,
            IManagementPermissionSupervisor supervisor,
            IMapper mapper,
            ILogger logger)
        {
            _hotelImageRepository = hotelImageRepository;
            _hotelRepository = hotelRepository;
            _supervisor = supervisor;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<HotelImageModel> CreateAsync(HotelImageModel imageModel)
        {
            _logger.Debug("Image (Hotel) is creating");

            await _supervisor.CheckHotelManagementPermissionAsync(imageModel.HotelId);

            var imageEntity = _mapper.Map<HotelImageEntity>(imageModel);

            if (imageEntity.IsMain)
            {
                await ChangeHotelMainImageAsync(imageEntity.HotelId, imageEntity);
            }

            var createdImageEntity = await _hotelImageRepository.CreateAsync(imageEntity);
            var createdImageModel = _mapper.Map<HotelImageModel>(createdImageEntity);

            _logger.Debug($"Image (Hotel) {createdImageEntity.Id} created");

            return createdImageModel;
        }

        public async Task<HotelImageModel> GetAsync(Guid id)
        {
            _logger.Debug($"Image (Hotel) {id} is requesting");

            var imageEntity = await _hotelImageRepository.GetAsync(id) ??
                              throw new BusinessException("Image with such id does not exist", ErrorStatus.NotFound);

            var imageModel = _mapper.Map<HotelImageModel>(imageEntity);

            _logger.Debug($"Image (Hotel) {id} requested");

            return imageModel;
        }

        public async Task<HotelImageModel> DeleteAsync(Guid id)
        {
            _logger.Debug($"Image (Hotel) {id} is deleting");

            var imageEntity = await _hotelImageRepository.GetAsync(id) ??
                              throw new BusinessException("No image with such id", ErrorStatus.NotFound);

            await _supervisor.CheckHotelManagementPermissionAsync(imageEntity.HotelId);

            var deletedImageEntity = await _hotelImageRepository.DeleteAsync(id);
            var deletedImageModel = _mapper.Map<HotelImageModel>(deletedImageEntity);

            _logger.Debug($"Image (Hotel) {id} deleted");

            return deletedImageModel;
        }

        public async Task<HotelImageModel> ChangeImageToMainAsync(Guid id)
        {
            _logger.Debug($"Image (Hotel) {id} is updating");

            var imageEntity = await _hotelImageRepository.GetAsync(id) ??
                              throw new BusinessException("No image with such id", ErrorStatus.NotFound);

            await _supervisor.CheckHotelManagementPermissionAsync(imageEntity.HotelId);
            await ChangeHotelMainImageAsync(imageEntity.HotelId, imageEntity);
            imageEntity.IsMain = true;

            var updatedImageEntity = await _hotelImageRepository.UpdateAsync(imageEntity);
            var imageModel = _mapper.Map<HotelImageModel>(updatedImageEntity);

            _logger.Debug($"Image (Hotel) {id} is updated");

            return imageModel;
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
