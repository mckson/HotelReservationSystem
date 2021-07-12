using AutoMapper;
using HotelReservation.API.Application.Queries.Room;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Room
{
    public class GetRoomByIdHandler : IRequestHandler<GetRoomByIdQuery, RoomResponseModel>, IRequest<RoomResponseModel>
    {
        private readonly IMapper _mapper;
        private readonly IRoomRepository _roomRepository;

        public GetRoomByIdHandler(
            IRoomRepository roomRepository,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public async Task<RoomResponseModel> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
        {
            var roomEntity = await _roomRepository.GetAsync(request.Id) ??
                             throw new BusinessException("No room with such id", ErrorStatus.NotFound);

            var roomResponse = _mapper.Map<RoomResponseModel>(roomEntity);

            return roomResponse;
        }
    }
}
