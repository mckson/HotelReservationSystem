using AutoMapper;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Filters;
using HotelReservation.Data.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotelReservation.Business.Services
{
    public class RoomsService : IRoomsService
    {
        private readonly IRoomRepository _roomsRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IHotelRepository _hotelRepository;
        private readonly IManagementPermissionSupervisor _supervisor;

        public RoomsService(
            IRoomRepository roomsRepository,
            IHotelRepository hotelRepository,
            IManagementPermissionSupervisor supervisor,
            IMapper mapper,
            ILogger logger)
        {
            _roomsRepository = roomsRepository;
            _hotelRepository = hotelRepository;
            _supervisor = supervisor;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RoomModel> CreateAsync(RoomModel roomModel)
        {
            _logger.Debug("Room is creating");

            var roomEntity = _mapper.Map<RoomEntity>(roomModel);

            if (roomEntity.HotelId != null)
            {
                var hotelEntity = await _hotelRepository.GetAsync(roomEntity.HotelId.Value) ??
                                  throw new BusinessException("No hotel with such id", ErrorStatus.NotFound);

                await _supervisor.CheckHotelManagementPermissionAsync(hotelEntity.Id);

                if (hotelEntity.Rooms.Any(room => room.RoomNumber == roomEntity.RoomNumber))
                    throw new BusinessException("Hotel already has room with such number", ErrorStatus.AlreadyExist);

                if (roomModel.FloorNumber > hotelEntity.NumberFloors)
                    throw new BusinessException($"There are only {hotelEntity.NumberFloors} floors in {hotelEntity.Name}", ErrorStatus.IncorrectInput);
            }
            else
            {
                throw new BusinessException(
                    "Hotel id was null. Room cannot be created without hotel",
                    ErrorStatus.NotFound);
            }

            var createdRoomEntity = await _roomsRepository.CreateAsync(roomEntity);
            var createdRoomModel = _mapper.Map<RoomModel>(createdRoomEntity);

            _logger.Debug($"Room {createdRoomModel.Id} created");

            return createdRoomModel;
        }

        public async Task<RoomModel> GetAsync(Guid id)
        {
            _logger.Debug($"Room {id} is requesting");

            var roomEntity = await _roomsRepository.GetAsync(id) ??
                             throw new BusinessException("No room with such id", ErrorStatus.NotFound);

            var roomModel = _mapper.Map<RoomModel>(roomEntity);

            _logger.Debug($"Room {id} requested");

            return roomModel;
        }

        public async Task<RoomModel> DeleteAsync(Guid id)
        {
            _logger.Debug($"Room {id} is deleting");

            // was as no tracking
            var roomEntity = await _roomsRepository.GetAsync(id) ??
                             throw new BusinessException("No room with such id", ErrorStatus.NotFound);

            if (roomEntity.HotelId != null)
            {
                await _supervisor.CheckHotelManagementPermissionAsync(roomEntity.HotelId.Value);
            }
            else
            {
                throw new BusinessException(
                    "Hotel id was null. Room cannot be created without hotel",
                    ErrorStatus.NotFound);
            }

            var deletedRoomEntity = await _roomsRepository.DeleteAsync(id);
            var deletedRoomModel = _mapper.Map<RoomModel>(deletedRoomEntity);

            _logger.Debug($"Room {id} deleted");

            return deletedRoomModel;
        }

        public async Task<RoomModel> UpdateAsync(Guid id, RoomModel updatingRoomModel)
        {
            _logger.Debug($"Room {id} is updating");

            // was as no tracking
            var roomEntity = await _roomsRepository.GetAsync(id) ??
                             throw new BusinessException("No room with such id", ErrorStatus.NotFound);

            if (roomEntity.HotelId != null)
            {
                await _supervisor.CheckHotelManagementPermissionAsync(roomEntity.HotelId.Value);

                // was as no tracking
                var hotelEntity = await _hotelRepository.GetAsync(roomEntity.HotelId.Value);

                if (updatingRoomModel.FloorNumber > hotelEntity.NumberFloors)
                {
                    throw new BusinessException(
                        $"There are only {hotelEntity.NumberFloors} floors in {hotelEntity.Name}",
                        ErrorStatus.IncorrectInput);
                }

                if (hotelEntity.Rooms.Any(room => room.RoomNumber == updatingRoomModel.RoomNumber && room.Id != id))
                {
                    throw new BusinessException("Hotel already has room with such number", ErrorStatus.AlreadyExist);
                }
            }
            else
            {
                throw new BusinessException(
                    "Hotel id was null. Room cannot be created without hotel",
                    ErrorStatus.NotFound);
            }

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

        public async Task<int> GetCountAsync(RoomsFilter filter)
        {
            _logger.Debug("Rooms count are requesting");

            var count = await _roomsRepository.GetCountAsync(FilterExpression(filter));

            _logger.Debug("Rooms count are requested");
            return count;
        }

        public IEnumerable<RoomModel> GetPagedRooms(
            PaginationFilter paginationFilter,
            RoomsFilter filter)
        {
            _logger.Debug($"Paged rooms are requesting, page: {paginationFilter.PageNumber}, size: {paginationFilter.PageSize}");

            var roomEntities = _roomsRepository.Find(FilterExpression(filter), paginationFilter);
            var roomModels = _mapper.Map<IEnumerable<RoomModel>>(roomEntities);

            _logger.Debug($"Paged rooms are requested, page: {paginationFilter.PageNumber}, size: {paginationFilter.PageSize}");

            return roomModels;
        }

        private static Expression<Func<RoomEntity, bool>> FilterExpression(RoomsFilter filter)
        {
            return room =>
                room.HotelId.Value.Equals(filter.HotelId) &&
                ((!filter.DateIn.HasValue || !filter.DateOut.HasValue) || !room.ReservationRooms.Any(rr =>
                        (rr.Reservation.DateIn >= filter.DateIn && rr.Reservation.DateIn < filter.DateOut) ||
                        (rr.Reservation.DateOut > filter.DateIn && rr.Reservation.DateOut <= filter.DateOut)));
        }
    }
}
