using AutoMapper;
using HotelReservation.API.Application.Queries.Hotel;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Hotel
{
    public class GetHotelByIdHandler : IRequestHandler<GetHotelByIdQuery, HotelResponseModel>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;

        public GetHotelByIdHandler(
            IHotelRepository hotelRepository,
            IMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        public async Task<HotelResponseModel> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
        {
            var hotelEntity = await _hotelRepository.GetAsync(request.Id) ??
                              throw new BusinessException($"Hotel with {request.Id} does not exist", ErrorStatus.NotFound);

            var hotelResponse = _mapper.Map<HotelResponseModel>(hotelEntity);

            // foreach (var hotelUser in hotelResponse.HotelUsers)
            // {
            //     await GetRolesForUserModelAsync(hotelUser.User);
            // }
            return hotelResponse;
        }
    }
}
