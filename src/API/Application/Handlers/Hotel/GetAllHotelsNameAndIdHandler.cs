using AutoMapper;
using HotelReservation.API.Application.Queries.Hotel;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Hotel
{
    public class GetAllHotelsNameAndIdHandler : IRequestHandler<GetAllHotelsNameAndIdQuery, IEnumerable<HotelBriefResponse>>
    {
        private IHotelRepository _hotelRepository;
        private IMapper _mapper;

        public GetAllHotelsNameAndIdHandler(IHotelRepository hotelRepository, IMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<HotelBriefResponse>> Handle(GetAllHotelsNameAndIdQuery request, CancellationToken cancellationToken)
        {
            var hotelEntities = await Task.FromResult(_hotelRepository.GetAll()) ??
                                throw new BusinessException("No hotels were created yet", ErrorStatus.NotFound);

            var hotelResponses = _mapper.Map<IEnumerable<HotelBriefResponse>>(hotelEntities);

            return hotelResponses;
        }
    }
}
