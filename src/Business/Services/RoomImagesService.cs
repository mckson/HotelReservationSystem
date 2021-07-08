using AutoMapper;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Serilog;
using System;
using System.Threading.Tasks;

namespace HotelReservation.Business.Services
{
    public class RoomImagesService : IRoomImagesService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        // private readonly IHotelRepository _hotelRepository;
        // private readonly IManagementPermissionSupervisor _supervisor;
        private readonly IRoomImageRepository _roomImageRepository;

        public RoomImagesService(
            IRoomImageRepository roomImageRepository,
            // IHotelRepository hotelRepository,
            // IManagementPermissionSupervisor supervisor,
            IMapper mapper,
            ILogger logger)
        {
            _roomImageRepository = roomImageRepository;
            // _hotelRepository = hotelRepository;
            // _supervisor = supervisor;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RoomImageModel> CreateAsync(RoomImageModel imageModel)
        {
            _logger.Debug("Image (Room) is creating");

            // await _supervisor.CheckHotelManagementPermissionAsync(imageModel.HotelId);
            var imageEntity = _mapper.Map<RoomImageEntity>(imageModel);

            var createdImageEntity = await _roomImageRepository.CreateAsync(imageEntity);
            var createdImageModel = _mapper.Map<RoomImageModel>(createdImageEntity);

            _logger.Debug($"Image (Room) {createdImageEntity.Id} created");

            return createdImageModel;
        }

        public async Task<RoomImageModel> GetAsync(Guid id)
        {
            _logger.Debug($"Image (Room) {id} is requesting");

            var imageEntity = await _roomImageRepository.GetAsync(id) ??
                              throw new BusinessException("Image with such id does not exist", ErrorStatus.NotFound);

            var imageModel = _mapper.Map<RoomImageModel>(imageEntity);

            _logger.Debug($"Image (Room) {id} requested");

            return imageModel;
        }

        public async Task<RoomImageModel> DeleteAsync(Guid id)
        {
            _logger.Debug($"Image (Room) {id} is deleting");

            var imageEntity = await _roomImageRepository.GetAsync(id) ??
                              throw new BusinessException("No image with such id", ErrorStatus.NotFound);

            // await _supervisor.CheckHotelManagementPermissionAsync(imageEntity.HotelId);
            var deletedImageEntity = await _roomImageRepository.DeleteAsync(imageEntity.Id);
            var deletedImageModel = _mapper.Map<RoomImageModel>(deletedImageEntity);

            _logger.Debug($"Image (Room) {id} deleted");

            return deletedImageModel;
        }
    }
}
