using AutoMapper;
using HotelReservation.API.Application.Commands.RoomView;
using HotelReservation.API.Application.Interfaces;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.RoomView
{
    public class CreateRoomViewHandler : IRequestHandler<CreateRoomViewCommand, RoomViewResponseModel>
    {
        private readonly IRoomViewRepository _roomViewRepository;
        private readonly IMapper _mapper;
        private readonly IRoomViewHelper _roomViewHelper;

        public CreateRoomViewHandler(
            IRoomViewRepository roomViewRepository,
            IMapper mapper,
            IRoomViewHelper roomViewHelper)
        {
            _roomViewRepository = roomViewRepository;
            _mapper = mapper;
            _roomViewHelper = roomViewHelper;
        }

        public async Task<RoomViewResponseModel> Handle(CreateRoomViewCommand request, CancellationToken cancellationToken)
        {
            var isNameAvailable = _roomViewHelper.IsNameAvailable(request.Name);
            if (!isNameAvailable)
            {
                throw new BusinessException(
                    $"View with name {request.Name} already exists",
                    ErrorStatus.AlreadyExist);
            }

            var roomViewEntity = _mapper.Map<RoomViewEntity>(request);
            var createdRoomViewEntity = await _roomViewRepository.CreateAsync(roomViewEntity);
            var createdRoomViewResponse = _mapper.Map<RoomViewResponseModel>(createdRoomViewEntity);

            return createdRoomViewResponse;
        }
    }
}
