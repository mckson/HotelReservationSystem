using AutoMapper;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelReservation.Business.Services
{
    public class ImagesService : IImageService
    {
        private readonly IRepository<ImageEntity> _imagesRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IHotelRepository _hotelRepository;
        private readonly ManagementPermissionSupervisor _supervisor;

        public ImagesService(
            IRepository<ImageEntity> imagesRepository,
            IHotelRepository hotelRepository,
            ManagementPermissionSupervisor supervisor,
            IMapper mapper,
            ILogger logger)
        {
            _imagesRepository = imagesRepository;
            _hotelRepository = hotelRepository;
            _supervisor = supervisor;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ImageModel> CreateAsync(ImageModel imageModel, IEnumerable<Claim> userClaims)
        {
            _logger.Debug("Image is creating");

            await _supervisor.CheckHotelManagementPermissionAsync(imageModel.HotelId, userClaims);

            var imageEntity = _mapper.Map<ImageEntity>(imageModel);

            if (imageEntity.IsMain)
            {
                await ChangeHotelMainImageAsync(imageEntity.HotelId, imageEntity);
            }

            var createdImageEntity = await _imagesRepository.CreateAsync(imageEntity);
            var createdImageModel = _mapper.Map<ImageModel>(createdImageEntity);

            _logger.Debug($"Image {createdImageEntity.Id} created");

            return createdImageModel;
        }

        public async Task<ImageModel> GetAsync(int id)
        {
            _logger.Debug($"Image {id} is requesting");

            var imageEntity = await _imagesRepository.GetAsync(id) ??
                              throw new BusinessException("Image with such id does not exist", ErrorStatus.NotFound);

            var imageModel = _mapper.Map<ImageModel>(imageEntity);

            _logger.Debug($"Image {id} requested");

            return imageModel;
        }

        public async Task<ImageModel> DeleteAsync(int id, IEnumerable<Claim> userClaims)
        {
            _logger.Debug($"Image {id} is deleting");

            var imageEntity = await _imagesRepository.GetAsync(id) ??
                              throw new BusinessException("No image with such id", ErrorStatus.NotFound);

            await _supervisor.CheckHotelManagementPermissionAsync(imageEntity.HotelId, userClaims);

            var deletedImageEntity = await _imagesRepository.DeleteAsync(id);
            var deletedImageModel = _mapper.Map<ImageModel>(deletedImageEntity);

            _logger.Debug($"Image {id} deleted");

            return deletedImageModel;
        }

        public async Task<ImageModel> ChangeImageToMainAsync(int id, IEnumerable<Claim> userClaims)
        {
            _logger.Debug($"Image {id} is updating");

            var imageEntity = await _imagesRepository.GetAsync(id) ??
                              throw new BusinessException("No image with such id", ErrorStatus.NotFound);

            await _supervisor.CheckHotelManagementPermissionAsync(imageEntity.HotelId, userClaims);
            await ChangeHotelMainImageAsync(imageEntity.HotelId, imageEntity);
            imageEntity.IsMain = true;

            var updatedImageEntity = await _imagesRepository.UpdateAsync(imageEntity);
            var imageModel = _mapper.Map<ImageModel>(updatedImageEntity);

            _logger.Debug($"Image {id} is updated");

            return imageModel;
        }

        private async Task ChangeHotelMainImageAsync(int hotelId, ImageEntity newImage)
        {
            var hotelEntity = await _hotelRepository.GetAsync(hotelId) ??
                              throw new BusinessException("Hotel does not exists", ErrorStatus.NotFound);

            _logger.Debug($"Main image of hotel {hotelEntity.Id} is updating");

            var oldImage = _imagesRepository.Find(image => image.IsMain && image.HotelId == hotelEntity.Id).FirstOrDefault();

            if (oldImage != null && newImage != null)
            {
                oldImage.IsMain = false;
                await _imagesRepository.UpdateAsync(oldImage);
            }

            /*else
            {
                if (newImage != null)
                {
                    newImage.IsMain = true;
                    hotelEntity.Images.Add(newImage);
                    // await _imageRepository.CreateAsync(newImage);
                }
                else if (oldImage != null)
                {
                    hotelEntity.Images.Remove(oldImage);
                    // await _imageRepository.DeleteAsync(oldImage.Id);
                }
            }*/

            _logger.Debug($"Main image of hotel {hotelEntity.Id} is updated");
        }
    }
}
