using AutoMapper;
using HotelReservation.API.Application.Commands.Image;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Image
{
    public class CreateRoomImageHandler : IRequestHandler<CreateRoomImageCommand>
    {
        private readonly IRoomImageRepository _roomImageRepository;
        private readonly IMapper _mapper;

        public CreateRoomImageHandler(
            IRoomImageRepository roomImageRepository,
            IMapper mapper)
        {
            _roomImageRepository = roomImageRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(CreateRoomImageCommand request, CancellationToken cancellationToken)
        {
            // await _supervisor.CheckHotelManagementPermissionAsync(imageModel.HotelId);
            var imageEntity = _mapper.Map<RoomImageEntity>(request);

            await _roomImageRepository.CreateAsync(imageEntity);

            return Unit.Value;
        }
    }
}
