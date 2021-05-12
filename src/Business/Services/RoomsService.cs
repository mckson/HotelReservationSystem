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
            var roomEntity = _mapper.Map<RoomEntity>(roomModel);

            var hotelEntity = await _hotelRepository.GetAsync(roomEntity.HotelId) ??
                              throw new BusinessException("No hotel with such id", ErrorStatus.NotFound);

            await CheckHotelManagementPermissionAsync(hotelEntity.Id, userClaims);

            if (hotelEntity.Rooms.Any(room => room.RoomNumber == roomEntity.RoomNumber))
                throw new BusinessException("Hotel already has room with such number", ErrorStatus.AlreadyExist);

            var createdRoomEntity = await _roomsRepository.CreateAsync(roomEntity);
            var createdRoomModel = _mapper.Map<RoomModel>(createdRoomEntity);

            return createdRoomModel;
        }

        public async Task<RoomModel> GetAsync(int id)
        {
            var roomEntity = await _roomsRepository.GetAsync(id) ??
                             throw new BusinessException("No room with such id", ErrorStatus.NotFound);

            var roomModel = _mapper.Map<RoomModel>(roomEntity);

            return roomModel;
        }

        public async Task<RoomModel> DeleteAsync(int id, IEnumerable<Claim> userClaims)
        {
            var roomEntity = await _roomsRepository.GetAsync(id, true) ??
                             throw new BusinessException("No room with such id", ErrorStatus.NotFound);

            await CheckHotelManagementPermissionAsync(roomEntity.HotelId, userClaims);

            var deletedRoomEntity = await _roomsRepository.DeleteAsync(id);
            var deletedRoomModel = _mapper.Map<RoomModel>(deletedRoomEntity);

            return deletedRoomModel;
        }

        public async Task<RoomModel> UpdateAsync(int id, RoomModel updatingRoomModel, IEnumerable<Claim> userClaims)
        {
            var roomEntity = await _roomsRepository.GetAsync(id, true) ??
                             throw new BusinessException("No room with such id", ErrorStatus.NotFound);

            await CheckHotelManagementPermissionAsync(roomEntity.HotelId, userClaims);

            var updatingRoomEntity = _mapper.Map<RoomEntity>(updatingRoomModel);
            updatingRoomEntity.Id = id;

            RoomEntity updatedRoomEntity;
            try
            {
                updatedRoomEntity = await _roomsRepository.UpdateAsync(updatingRoomEntity);
            }
            catch
            {
                throw new BusinessException("Updating error! No room with such id", ErrorStatus.NotFound);
            }

            var updatedRoomModel = _mapper.Map<RoomModel>(updatedRoomEntity);

            return updatedRoomModel;
        }

        public IEnumerable<RoomModel> GetAllRooms()
        {
            var roomEntities = _roomsRepository.GetAll() ??
                               throw new BusinessException("No rooms were created", ErrorStatus.NotFound);
            var roomModels = _mapper.Map<IEnumerable<RoomModel>>(roomEntities);

            return roomModels;
        }

        public async Task<IEnumerable<RoomModel>> GetRoomsByHotelNameAsync(string hotelName)
        {
            var hotelEntity = await _hotelRepository.GetAsync(hotelName) ??
                              throw new BusinessException($"There is hotel with name {hotelName}", ErrorStatus.NotFound);

            var roomsEntities = _roomsRepository.Find(room => room.Hotel.Name == hotelName) ??
                                throw new BusinessException(
                                    $"No room in hotel {hotelName}",
                                    ErrorStatus.NotFound);
            var roomModels = _mapper.Map<IEnumerable<RoomModel>>(roomsEntities);

            return roomModels;
        }

        public async Task<IEnumerable<RoomModel>> GetRoomsByHotelIdAsync(int hotelId)
        {
            var hotelEntity = await _hotelRepository.GetAsync(hotelId) ??
                              throw new BusinessException($"There is hotel with id {hotelId}", ErrorStatus.NotFound);

            var roomsEntities = _roomsRepository.Find(room => room.HotelId == hotelId) ??
                                throw new BusinessException(
                                    $"No room in hotel with id {hotelEntity}",
                                    ErrorStatus.NotFound);
            var roomModels = _mapper.Map<IEnumerable<RoomModel>>(roomsEntities);

            return roomModels;
        }

        private async Task CheckHotelManagementPermissionAsync(int id, IEnumerable<Claim> userClaims)
        {
            if (userClaims.Where(claim => claim.Type.Equals(ClaimTypes.Role)).Any(role => role.Value.ToUpper() == "ADMIN"))
                return;

            var hotelEntity = await _hotelRepository.GetAsync(id, true) ??
                              throw new BusinessException("No hotel with such id", ErrorStatus.NotFound);

            var hotelIdString = userClaims.FirstOrDefault(claim => claim.Type == "hotelId")?.Value;
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
        }
    }
}
