using AutoMapper;
using HotelReservation.API.Application.Commands.Room;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Business.Interfaces;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Room
{
    public class UpdateRoomHandler : IRequestHandler<UpdateRoomCommand, RoomResponseModel>
    {
        private readonly IMapper _mapper;
        private readonly IRoomRepository _roomRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IRoomViewRepository _roomViewRepository;
        private readonly IManagementPermissionSupervisor _supervisor;

        public UpdateRoomHandler(
            IMapper mapper,
            IRoomRepository roomRepository,
            IManagementPermissionSupervisor supervisor,
            IHotelRepository hotelRepository,
            IRoomViewRepository roomViewRepository)
        {
            _mapper = mapper;
            _roomRepository = roomRepository;
            _supervisor = supervisor;
            _hotelRepository = hotelRepository;
            _roomViewRepository = roomViewRepository;
        }

        public async Task<RoomResponseModel> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
        {
            var roomEntity = await _roomRepository.GetAsync(request.Id) ??
                             throw new BusinessException("No room with such id", ErrorStatus.NotFound);

            if (roomEntity.HotelId != null)
            {
                await _supervisor.CheckHotelManagementPermissionAsync(roomEntity.HotelId.Value);

                var hotelEntity = await _hotelRepository.GetAsync(roomEntity.HotelId.Value);

                if (request.FloorNumber > hotelEntity.NumberFloors)
                {
                    throw new BusinessException(
                        $"There are only {hotelEntity.NumberFloors} floors in {hotelEntity.Name}",
                        ErrorStatus.IncorrectInput);
                }

                if (hotelEntity.Rooms.Any(room => room.RoomNumber == request.RoomNumber && room.Id != request.Id))
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

            roomEntity.Name = request.Name;
            roomEntity.RoomNumber = request.RoomNumber;
            roomEntity.FloorNumber = request.FloorNumber;
            roomEntity.Price = request.Price;
            roomEntity.Capacity = request.Capacity;
            roomEntity.Area = request.Area;
            roomEntity.Description = request.Description;
            roomEntity.Smoking = request.Smoking;
            roomEntity.Parking = request.Parking;

            roomEntity.Facilities.RemoveAll(f => f.RoomId == request.Id);
            // roomEntity.Facilities = _mapper.Map<List<RoomFacilityEntity>>(request.Facilities);
            roomEntity.Facilities =
                request.Facilities.Select(facilityName => new RoomFacilityEntity { Name = facilityName }).ToList();

            var roomRoomViews = new List<RoomRoomViewEntity>();
            roomEntity.RoomViews.RemoveAll(rr => rr.RoomId == roomEntity.Id);

            if (request.Views != null)
            {
                foreach (var roomViewId in request.Views)
                {
                    var unused = await _roomViewRepository.GetAsync(roomViewId) ??
                                 throw new BusinessException("There is no room view with such id", ErrorStatus.NotFound);

                    roomRoomViews.Add(new RoomRoomViewEntity() { RoomViewId = roomViewId });
                }
            }

            if (roomRoomViews.Count > 0)
            {
                roomEntity.RoomViews = roomRoomViews;
            }

            // roomEntity.RoomViews = _mapper.Map<IEnumerable<RoomRoomViewEntity>>(updatingRoomModel.RoomViews);
            RoomEntity updatedRoomEntity;
            try
            {
                updatedRoomEntity = await _roomRepository.UpdateAsync(roomEntity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ErrorStatus.IncorrectInput);
            }

            var updatedRoomResponse = _mapper.Map<RoomResponseModel>(updatedRoomEntity);

            return updatedRoomResponse;
        }
    }
}
