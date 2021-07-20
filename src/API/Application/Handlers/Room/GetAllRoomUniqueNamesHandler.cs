using HotelReservation.API.Application.Queries.Room;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Room
{
    public class GetAllRoomUniqueNamesHandler : IRequestHandler<GetAllRoomUniqueNamesQuery, IEnumerable<string>>
    {
        private readonly IRoomRepository _roomRepository;

        public GetAllRoomUniqueNamesHandler(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<IEnumerable<string>> Handle(GetAllRoomUniqueNamesQuery request, CancellationToken cancellationToken)
        {
            var names = await Task.FromResult(_roomRepository.GetAllRoomsGetAllUniqueNames());
            return names;
        }
    }
}
