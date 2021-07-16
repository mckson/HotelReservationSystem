using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HotelReservation.API.Application.Queries.Room;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Data.Interfaces;
using MediatR;

namespace HotelReservation.API.Application.Handlers.Room
{
    public class GetAllRoomsHandler : IRequestHandler<GetAllRoomsQuery, IEnumerable<RoomResponseModel>>
    {
        private readonly IMapper _mapper;
        private readonly IRoomRepository _roomRepository;

        public GetAllRoomsHandler(
            IRoomRepository roomRepository,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoomResponseModel>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
        {
            var roomEntities = await Task.FromResult(_roomRepository.GetAll());
            var roomResponses = _mapper.Map<IEnumerable<RoomResponseModel>>(roomEntities);

            return roomResponses;
        }
    }
}
