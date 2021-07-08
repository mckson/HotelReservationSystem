using AutoMapper;
using HotelReservation.API.Commands.RoomView;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Handlers.RoomView
{
    public class CreateRoomViewHandler : IRequestHandler<CreateRoomViewCommand, RoomViewResponseModel>
    {
        private readonly IRoomViewRepository _roomViewRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CreateRoomViewHandler(IRoomViewRepository roomViewRepository, ILogger logger, IMapper mapper)
        {
            _roomViewRepository = roomViewRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<RoomViewResponseModel> Handle(CreateRoomViewCommand request, CancellationToken cancellationToken)
        {
            _logger.Debug($"Room view {request.Name} is creating");

            var isNameAvailable = IsNameAvailable(request.Name);
            if (!isNameAvailable)
            {
                throw new BusinessException(
                    $"View with name {request.Name} already exists",
                    ErrorStatus.AlreadyExist);
            }

            var roomViewEntity = _mapper.Map<RoomViewEntity>(request);
            var createdRoomViewEntity = await _roomViewRepository.CreateAsync(roomViewEntity);
            var createdRoomViewResponse = _mapper.Map<RoomViewResponseModel>(createdRoomViewEntity);

            _logger.Debug($"Room view {request.Name} is created");

            return createdRoomViewResponse;
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
