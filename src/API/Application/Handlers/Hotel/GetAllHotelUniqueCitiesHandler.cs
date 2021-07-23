using HotelReservation.API.Application.Queries.Hotel;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Hotel
{
    public class GetAllHotelUniqueCitiesHandler : IRequestHandler<GetAllHotelUniqueCitiesQuery, IEnumerable<string>>
    {
        private readonly IHotelRepository _hotelRepository;

        public GetAllHotelUniqueCitiesHandler(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task<IEnumerable<string>> Handle(
            GetAllHotelUniqueCitiesQuery request,
            CancellationToken cancellationToken)
        {
            var cities = await Task.FromResult(_hotelRepository.GetAllHotelsUniqueCities());
            return cities;
        }
    }
}
