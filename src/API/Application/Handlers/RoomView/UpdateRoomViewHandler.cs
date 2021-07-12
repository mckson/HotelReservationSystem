using AutoMapper;
using HotelReservation.API.Application.Commands.RoomView;
using HotelReservation.API.Application.Interfaces;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.RoomView
{
    public class UpdateRoomViewHandler : IRequestHandler<UpdateRoomViewCommand, RoomViewResponseModel>
    {
        private readonly IRoomViewRepository _roomViewRepository;
        private readonly IMapper _mapper;
        private readonly IRoomViewHelper _roomViewHelper;

        public UpdateRoomViewHandler(
            IRoomViewRepository roomViewRepository,
            IMapper mapper,
            IRoomViewHelper roomViewHelper)
        {
            _roomViewRepository = roomViewRepository;
            _mapper = mapper;
            _roomViewHelper = roomViewHelper;
        }

        public async Task<RoomViewResponseModel> Handle(UpdateRoomViewCommand request, CancellationToken cancellationToken)
        {
            var isNameAvailable = _roomViewHelper.IsNameAvailable(request.Name);
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

            return updatedRoomViewResponse;
        }
    }
}
