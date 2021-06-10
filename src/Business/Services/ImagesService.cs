using AutoMapper;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Serilog;
using System.Threading.Tasks;

namespace HotelReservation.Business.Services
{
    public class ImagesService : IImageService
    {
        private readonly IRepository<ImageEntity> _imagesRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ImagesService(
            IRepository<ImageEntity> imagesRepository,
            IMapper mapper,
            ILogger logger)
        {
            _imagesRepository = imagesRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ImageModel> CreateAsync(ImageModel imageModel)
        {
            _logger.Debug("Image is creating");

            var imageEntity = _mapper.Map<ImageEntity>(imageModel);
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

        public async Task<ImageModel> DeleteAsync(int id)
        {
            _logger.Debug($"Image {id} is deleting");

            var imageEntity = await _imagesRepository.GetAsync(id) ??
                              throw new BusinessException("No image with such id", ErrorStatus.NotFound);

            var deletedImageEntity = await _imagesRepository.DeleteAsync(id);
            var deletedImageModel = _mapper.Map<ImageModel>(deletedImageEntity);

            _logger.Debug($"Image {id} deleted");

            return deletedImageModel;
        }
    }
}
