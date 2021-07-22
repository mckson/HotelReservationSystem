using HotelReservation.API.Application.Queries.Hotel;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Hotel
{
    public class GetAllHotelUniqueNamesHandler : IRequestHandler<GetAllHotelUniqueNamesQuery, IEnumerable<string>>
    {
        private readonly IHotelRepository _hotelRepository;

        public GetAllHotelUniqueNamesHandler(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task<IEnumerable<string>> Handle(GetAllHotelUniqueNamesQuery request, CancellationToken cancellationToken)
        {
            var names = await Task.FromResult(_hotelRepository.GetAllHotelsUniqueNames());
            return names;
        }
    }
}
