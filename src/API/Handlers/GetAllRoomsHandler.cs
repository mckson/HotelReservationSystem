using AutoMapper;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.API.Queries;
using HotelReservation.Business.Models;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Handlers
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
            var roomEntities = _roomRepository.GetAll();
            var roomModels = _mapper.Map<IEnumerable<RoomModel>>(roomEntities);
            var roomResponses = _mapper.Map<IEnumerable<RoomResponseModel>>(roomModels);

            return roomResponses;
        }
    }
}
