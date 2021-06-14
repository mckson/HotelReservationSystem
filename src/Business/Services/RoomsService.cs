using System;
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
    public class RoomsService : IRoomsService
    {
        private readonly IRepository<RoomEntity> _roomsRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IHotelRepository _hotelRepository;

        public RoomsService(
            IRepository<RoomEntity> roomsRepository,
            IHotelRepository hotelRepository,
            IMapper mapper,
            ILogger logger)
        {
            _roomsRepository = roomsRepository;
            _hotelRepository = hotelRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RoomModel> CreateAsync(RoomModel roomModel, IEnumerable<Claim> userClaims)
        {
            _logger.Debug("Room is creating");

            var roomEntity = _mapper.Map<RoomEntity>(roomModel);

            var hotelEntity = await _hotelRepository.GetAsync(roomEntity.HotelId.Value) ??
                              throw new BusinessException("No hotel with such id", ErrorStatus.NotFound);

            await CheckHotelManagementPermissionAsync(hotelEntity.Id, userClaims);

            if (hotelEntity.Rooms.Any(room => room.RoomNumber == roomEntity.RoomNumber))
                throw new BusinessException("Hotel already has room with such number", ErrorStatus.AlreadyExist);

            if (roomModel.FloorNumber > hotelEntity.NumberFloors)
                throw new BusinessException($"There are only {hotelEntity.NumberFloors} floors in {hotelEntity.Name}", ErrorStatus.IncorrectInput);

            var createdRoomEntity = await _roomsRepository.CreateAsync(roomEntity);
            var createdRoomModel = _mapper.Map<RoomModel>(createdRoomEntity);

            _logger.Debug($"Room {createdRoomModel.Id} created");

            return createdRoomModel;
        }

        public async Task<RoomModel> GetAsync(int id)
        {
            _logger.Debug($"Room {id} is requesting");

            var roomEntity = await _roomsRepository.GetAsync(id) ??
                             throw new BusinessException("No room with such id", ErrorStatus.NotFound);

            var roomModel = _mapper.Map<RoomModel>(roomEntity);

            _logger.Debug($"Room {id} requested");

            return roomModel;
        }

        public async Task<RoomModel> DeleteAsync(int id, IEnumerable<Claim> userClaims)
        {
            _logger.Debug($"Room {id} is deleting");

            // was as no tracking
            var roomEntity = await _roomsRepository.GetAsync(id) ??
                             throw new BusinessException("No room with such id", ErrorStatus.NotFound);

            await CheckHotelManagementPermissionAsync(roomEntity.HotelId.Value, userClaims);

            var deletedRoomEntity = await _roomsRepository.DeleteAsync(id);
            var deletedRoomModel = _mapper.Map<RoomModel>(deletedRoomEntity);

            _logger.Debug($"Room {id} deleted");

            return deletedRoomModel;
        }

        public async Task<RoomModel> UpdateAsync(int id, RoomModel updatingRoomModel, IEnumerable<Claim> userClaims)
        {
            _logger.Debug($"Room {id} is updating");

            // was as no tracking
            var roomEntity = await _roomsRepository.GetAsync(id) ??
                             throw new BusinessException("No room with such id", ErrorStatus.NotFound);

            await CheckHotelManagementPermissionAsync(roomEntity.HotelId.Value, userClaims);

            // was as no tracking
            var hotelEntity = await _hotelRepository.GetAsync(roomEntity.HotelId.Value);

            if (updatingRoomModel.FloorNumber > hotelEntity.NumberFloors)
                throw new BusinessException($"There are only {hotelEntity.NumberFloors} floors in {hotelEntity.Name}", ErrorStatus.IncorrectInput);

            if (hotelEntity.Rooms.Any(room => room.RoomNumber == updatingRoomModel.RoomNumber && room.Id != id))
                throw new BusinessException("Hotel already has room with such number", ErrorStatus.AlreadyExist);

            roomEntity.RoomNumber = updatingRoomModel.RoomNumber;
            roomEntity.FloorNumber = updatingRoomModel.FloorNumber;
            roomEntity.Price = updatingRoomModel.Price;
            roomEntity.Capacity = updatingRoomModel.Capacity;

            RoomEntity updatedRoomEntity;
            try
            {
                updatedRoomEntity = await _roomsRepository.UpdateAsync(roomEntity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ErrorStatus.IncorrectInput);
            }

            var updatedRoomModel = _mapper.Map<RoomModel>(updatedRoomEntity);

            _logger.Debug($"Room {id} updated");

            return updatedRoomModel;
        }

        public IEnumerable<RoomModel> GetAllRooms()
        {
            _logger.Debug("Rooms are requesting");

            var roomEntities = _roomsRepository.GetAll() ??
                               throw new BusinessException("No rooms were created", ErrorStatus.NotFound);
            var roomModels = _mapper.Map<IEnumerable<RoomModel>>(roomEntities);

            _logger.Debug("Rooms requested");

            return roomModels;
        }

        private async Task CheckHotelManagementPermissionAsync(int id, IEnumerable<Claim> userClaims)
        {
            _logger.Debug($"Permissions for managing hotel with id {id} is checking");

            var claims = userClaims.ToList();
            if (claims.Where(claim => claim.Type.Equals(ClaimTypes.Role)).Any(role => role.Value.ToUpper() == "ADMIN"))
                return;

            // was as no tracking
            var hotelEntity = await _hotelRepository.GetAsync(id) ??
                              throw new BusinessException($"No hotel with such id {id}", ErrorStatus.NotFound);

            var hotelIdString = claims.FirstOrDefault(claim => claim.Type == "hotelId")?.Value;
            int.TryParse(hotelIdString, out var hotelId);

            if (hotelId == 0)
            {
                throw new BusinessException(
                    "You have no permissions to manage hotels. Ask application admin to take that permission",
                    ErrorStatus.AccessDenied);
            }

            if (hotelId != hotelEntity.Id)
            {
                throw new BusinessException(
                    $"You have no permission to manage hotel {hotelEntity.Name}. Ask application admin about permissions",
                    ErrorStatus.AccessDenied);
            }

            _logger.Debug($"Permissions for managing hotel with id {id} checked");
        }
    }
}
