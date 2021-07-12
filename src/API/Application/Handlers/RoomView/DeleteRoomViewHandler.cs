using AutoMapper;
using HotelReservation.API.Application.Commands.RoomView;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.RoomView
{
    public class DeleteRoomViewHandler : IRequestHandler<DeleteRoomViewCommand, RoomViewResponseModel>
    {
        private readonly IRoomViewRepository _roomViewRepository;
        private readonly IMapper _mapper;

        public DeleteRoomViewHandler(
            IRoomViewRepository roomViewRepository,
            IMapper mapper)
        {
            _roomViewRepository = roomViewRepository;
            _mapper = mapper;
        }

        public async Task<RoomViewResponseModel> Handle(DeleteRoomViewCommand request, CancellationToken cancellationToken)
        {
            var roomViewEntity = await _roomViewRepository.GetAsync(request.Id) ?? throw new BusinessException(
                "Room view with such id does not exist",
                ErrorStatus.NotFound);

            var deletedRoomViewEntity = await _roomViewRepository.DeleteAsync(roomViewEntity.Id);
            var deletedRoomViewResponse = _mapper.Map<RoomViewResponseModel>(deletedRoomViewEntity);

            return deletedRoomViewResponse;
        }
    }
}
