using AutoMapper;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservation.Business.Services
{
    public class RoomViewsService : IRoomViewsService
    {
        private readonly IRoomViewRepository _roomViewRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public RoomViewsService(
            IRoomViewRepository roomViewRepository,
            ILogger logger,
            IMapper mapper)
        {
            _roomViewRepository = roomViewRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<RoomViewModel> CreateAsync(RoomViewModel roomViewModel)
        {
            _logger.Debug($"Room view {roomViewModel.Name} is creating");

            var roomViewEntity = _mapper.Map<RoomViewEntity>(roomViewModel);
            var createdRoomViewEntity = await _roomViewRepository.CreateAsync(roomViewEntity);
            var createdRoomViewModel = _mapper.Map<RoomViewModel>(createdRoomViewEntity);

            _logger.Debug($"Room view {roomViewModel.Name} is created");

            return createdRoomViewModel;
        }

        public async Task<RoomViewModel> GetAsync(Guid id)
        {
            _logger.Debug($"Room view {id} is requesting");

            var roomViewEntity = await _roomViewRepository.GetAsync(id) ?? throw new BusinessException(
                "Room view with such id does not exist",
                ErrorStatus.NotFound);

            var roomViewModel = _mapper.Map<RoomViewModel>(roomViewEntity);

            _logger.Debug($"Room view {id} is requested");

            return roomViewModel;
        }

        public async Task<RoomViewModel> DeleteAsync(Guid id)
        {
            _logger.Debug($"Room view {id} is deleting");

            var roomViewEntity = await _roomViewRepository.GetAsync(id) ?? throw new BusinessException(
                "Room view with such id does not exist",
                ErrorStatus.NotFound);

            var deletedRoomViewEntity = await _roomViewRepository.DeleteAsync(roomViewEntity.Id);
            var deletedRoomViewModel = _mapper.Map<RoomViewModel>(deletedRoomViewEntity);

            _logger.Debug($"Room view {id} is deleted");

            return deletedRoomViewModel;
        }

        public async Task<RoomViewModel> UpdateAsync(Guid id, RoomViewModel updatingRoomViewModel)
        {
            _logger.Debug($"Room view {id} is updating");

            var roomViewEntity = await _roomViewRepository.GetAsync(id) ?? throw new BusinessException(
                "Room view with such id does not exist",
                ErrorStatus.NotFound);

            roomViewEntity.Name = updatingRoomViewModel.Name;
            RoomViewEntity updatedRoomViewEntity;
            try
            {
                updatedRoomViewEntity = await _roomViewRepository.UpdateAsync(roomViewEntity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ErrorStatus.IncorrectInput);
            }

            var updatedRoomViewModel = _mapper.Map<RoomViewModel>(updatedRoomViewEntity);

            _logger.Debug($"Room view {id} is updated");

            return updatedRoomViewModel;
        }

        public IEnumerable<RoomViewModel> GetAllRoomViews()
        {
            _logger.Debug("Room views are requesting");

            var roomViewEntities = _roomViewRepository.GetAll() ??
                                   throw new BusinessException("No room views were created", ErrorStatus.NotFound);

            var roomViewModels = _mapper.Map<IEnumerable<RoomViewModel>>(roomViewEntities);

            _logger.Debug("Room views are requested");

            return roomViewModels;
        }
    }
}
