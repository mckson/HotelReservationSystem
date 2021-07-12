using AutoMapper;
using HotelReservation.API.Application.Commands.Room;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Business.Interfaces;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Room
{
    public class DeleteRoomHandler : IRequestHandler<DeleteRoomCommand, RoomResponseModel>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IManagementPermissionSupervisor _supervisor;
        private readonly IMapper _mapper;

        public DeleteRoomHandler(
            IRoomRepository roomRepository,
            IMapper mapper,
            IManagementPermissionSupervisor supervisor)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
            _supervisor = supervisor;
        }

        public async Task<RoomResponseModel> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
        {
            var roomEntity = await _roomRepository.GetAsync(request.Id) ??
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

            var deletedRoomEntity = await _roomRepository.DeleteAsync(request.Id);
            var deletedRoomResponse = _mapper.Map<RoomResponseModel>(deletedRoomEntity);

            return deletedRoomResponse;
        }
    }
}
