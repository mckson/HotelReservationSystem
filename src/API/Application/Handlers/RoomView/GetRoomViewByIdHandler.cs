using AutoMapper;
using HotelReservation.API.Application.Queries.RoomView;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.RoomView
{
    public class GetRoomViewByIdHandler : IRequestHandler<GetRoomViewByIdQuery, RoomViewResponseModel>
    {
        private readonly IRoomViewRepository _roomViewRepository;
        private readonly IMapper _mapper;

        public GetRoomViewByIdHandler(
            IRoomViewRepository roomViewRepository,
            IMapper mapper)
        {
            _roomViewRepository = roomViewRepository;
            _mapper = mapper;
        }

        public async Task<RoomViewResponseModel> Handle(GetRoomViewByIdQuery request, CancellationToken cancellationToken)
        {
            var roomViewEntity = await _roomViewRepository.GetAsync(request.Id) ?? throw new BusinessException(
                "Room view with such id does not exist",
                ErrorStatus.NotFound);

            var roomViewResponse = _mapper.Map<RoomViewResponseModel>(roomViewEntity);

            return roomViewResponse;
        }
    }
}
