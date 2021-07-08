using AutoMapper;
using HotelReservation.API.Commands.RoomView;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Handlers.RoomView
{
    public class UpdateRoomViewHandler : IRequestHandler<UpdateRoomViewCommand, RoomViewResponseModel>
    {
        private readonly IRoomViewRepository _roomViewRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public UpdateRoomViewHandler(IRoomViewRepository roomViewRepository, ILogger logger, IMapper mapper)
        {
            _roomViewRepository = roomViewRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<RoomViewResponseModel> Handle(UpdateRoomViewCommand request, CancellationToken cancellationToken)
        {
            _logger.Debug($"Room view {request} is updating");

            var isNameAvailable = IsNameAvailable(request.Name);
            if (!isNameAvailable)
            {
                throw new BusinessException(
                    $"View with name {request.Name} already exists",
                    ErrorStatus.AlreadyExist);
            }

            var roomViewEntity = await _roomViewRepository.GetAsync(request.Id) ?? throw new BusinessException(
                "Room view with such id does not exist",
                ErrorStatus.NotFound);

            roomViewEntity.Name = request.Name;
            RoomViewEntity updatedRoomViewEntity;
            try
            {
                updatedRoomViewEntity = await _roomViewRepository.UpdateAsync(roomViewEntity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ErrorStatus.IncorrectInput);
            }

            var updatedRoomViewResponse = _mapper.Map<RoomViewResponseModel>(updatedRoomViewEntity);

            _logger.Debug($"Room view {request.Id} is updated");

            return updatedRoomViewResponse;
        }

        private bool IsNameAvailable(string roomViewName)
        {
            var isNameAvailable = true;
            var roomViewEntity = _roomViewRepository.Find(view =>
                view.Name.ToUpper().Equals(roomViewName.ToUpper())).FirstOrDefault();

            if (roomViewEntity != null)
            {
                isNameAvailable = false;
            }

            return isNameAvailable;
        }
    }
}
