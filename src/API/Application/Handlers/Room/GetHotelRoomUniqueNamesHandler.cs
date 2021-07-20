using HotelReservation.API.Application.Queries.Room;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Room
{
    public class GetHotelRoomUniqueNamesHandler : IRequestHandler<GetHotelRoomsUniqueNamesQuery, IEnumerable<string>>
    {
        private readonly IRoomRepository _roomRepository;

        public GetHotelRoomUniqueNamesHandler(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<IEnumerable<string>> Handle(
            GetHotelRoomsUniqueNamesQuery request,
            CancellationToken cancellationToken)
        {
            var names = await Task.FromResult(_roomRepository.GetHotelRoomsUniqueNames(request.HotelId));
            return names;
        }
    }
}
