using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HotelReservation.API.Application.Commands.RoomView;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;

namespace HotelReservation.API.Application.Handlers.RoomView
{
    public class DeleteRoomViewHandler : IRequestHandler<DeleteRoomViewCommand, RoomViewResponseModel>
    {
        private readonly IRoomViewRepository _roomViewRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public DeleteRoomViewHandler(IRoomViewRepository roomViewRepository, ILogger logger, IMapper mapper)
        {
            _roomViewRepository = roomViewRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<RoomViewResponseModel> Handle(DeleteRoomViewCommand request, CancellationToken cancellationToken)
        {
            _logger.Debug($"Room view {request.Id} is deleting");

            var roomViewEntity = await _roomViewRepository.GetAsync(request.Id) ?? throw new BusinessException(
                "Room view with such id does not exist",
                ErrorStatus.NotFound);

            var deletedRoomViewEntity = await _roomViewRepository.DeleteAsync(roomViewEntity.Id);
            var deletedRoomViewResponse = _mapper.Map<RoomViewResponseModel>(deletedRoomViewEntity);

            _logger.Debug($"Room view {request.Id} is deleted");

            return deletedRoomViewResponse;
        }
    }
}
