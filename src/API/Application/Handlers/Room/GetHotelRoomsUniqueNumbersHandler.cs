using HotelReservation.API.Application.Queries.Room;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Room
{
    public class GetHotelRoomsUniqueNumbersHandler : IRequestHandler<GetHotelRoomsUniqueNumbersQuery, IEnumerable<int>>
    {
        private readonly IRoomRepository _roomRepository;

        public GetHotelRoomsUniqueNumbersHandler(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<IEnumerable<int>> Handle(GetHotelRoomsUniqueNumbersQuery request, CancellationToken cancellationToken)
        {
            var numbers = await Task.FromResult(_roomRepository.GetHotelRoomsUniqueNumbers(request.HotelId));
            return numbers;
        }
    }
}
