using HotelReservation.API.Application.Queries.Hotel;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Hotel
{
    public class GetHotelSearchNamesHandler : IRequestHandler<GetHotelSearchNamesQuery, IEnumerable<string>>
    {
        private readonly IHotelRepository _hotelRepository;

        public GetHotelSearchNamesHandler(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task<IEnumerable<string>> Handle(GetHotelSearchNamesQuery request, CancellationToken cancellationToken)
        {
            var hotelEntities = await Task.FromResult(_hotelRepository.Find(hotel => hotel.Name.StartsWith(request.SearchName)));
            var hotelNames = hotelEntities.OrderBy(hotel => hotel.Name).Select(hotel => hotel.Name);

            return hotelNames;
        }
    }
}
