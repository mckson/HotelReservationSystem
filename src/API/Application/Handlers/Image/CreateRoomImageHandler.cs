using AutoMapper;
using HotelReservation.API.Application.Commands.Image;
using HotelReservation.Business;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Image
{
    public class CreateRoomImageHandler : IRequestHandler<CreateRoomImageCommand>
    {
        private readonly IRoomImageRepository _roomImageRepository;
        private readonly IMapper _mapper;
        private readonly IRoomRepository _roomRepository;

        public CreateRoomImageHandler(
            IRoomImageRepository roomImageRepository,
            IMapper mapper,
            IRoomRepository roomRepository)
        {
            _roomImageRepository = roomImageRepository;
            _mapper = mapper;
            _roomRepository = roomRepository;
        }

        public async Task<Unit> Handle(CreateRoomImageCommand request, CancellationToken cancellationToken)
        {
            // await _supervisor.CheckHotelManagementPermissionAsync(imageModel.HotelId);
            var imageEntity = _mapper.Map<RoomImageEntity>(request);

            var roomEntity = await _roomRepository.GetAsync(Guid.Parse(request.RoomId));
            if (roomEntity == null)
            {
                throw new BusinessException($"Room with id {request.RoomId} does not exist", ErrorStatus.NotFound);
            }

            await _roomImageRepository.CreateAsync(imageEntity);

            return Unit.Value;
        }
    }
}
