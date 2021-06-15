using AutoMapper;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Serilog;
using System.Collections.Generic;
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

/*
        private async Task CheckHotelManagementPermissionAsync(int id, IEnumerable<Claim> userClaims)
        {
            _logger.Debug($"Permissions for managing hotel with id {id} is checking");

            var claims = userClaims.ToList();
            if (claims.Where(claim => claim.Type.Equals(ClaimTypes.Role)).Any(role => role.Value.ToUpper() == "ADMIN"))
                return;

            // was as no tracking
            var hotelEntity = await _hotelRepository.GetAsync(id) ??
                              throw new BusinessException("No hotel with such id", ErrorStatus.NotFound);

            var hotels = claims.FindAll(claim => claim.Type == ClaimNames.Hotels);

            if (hotels.Count == 0)
            {
                throw new BusinessException(
                    "You have no permissions to manage hotels. Ask application admin to take that permission",
                    ErrorStatus.AccessDenied);
            }

            var accessDenied = true;
            foreach (var hotel in hotels)
            {
                int.TryParse(hotel.Value, out var hotelId);

                if (hotelId != hotelEntity.Id)
                    continue;

                accessDenied = false;
                break;
            }

            if (accessDenied)
            {
                throw new BusinessException(
                    $"You have no permission to manage hotel {hotelEntity.Name}. Ask application admin about permissions",
                    ErrorStatus.AccessDenied);
            }

            _logger.Debug($"Permissions for managing hotel with id {id} checked");
        }
*/
    }
}
