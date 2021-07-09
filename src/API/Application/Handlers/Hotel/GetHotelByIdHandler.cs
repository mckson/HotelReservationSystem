using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HotelReservation.API.Application.Queries.Hotel;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;

namespace HotelReservation.API.Application.Handlers.Hotel
{
    public class GetHotelByIdHandler : IRequestHandler<GetHotelByIdQuery, HotelResponseModel>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public GetHotelByIdHandler(IHotelRepository hotelRepository, IMapper mapper, ILogger logger)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<HotelResponseModel> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.Debug($"Hotel with {request.Id} is getting");

            var hotelEntity = await _hotelRepository.GetAsync(request.Id) ??
                              throw new BusinessException($"Hotel with {request.Id} does not exist", ErrorStatus.NotFound);

            var hotelResponse = _mapper.Map<HotelResponseModel>(hotelEntity);

            // foreach (var hotelUser in hotelResponse.HotelUsers)
            // {
            //     await GetRolesForUserModelAsync(hotelUser.User);
            // }
            _logger.Debug($"Hotel with {request.Id} was get");

            return hotelResponse;
        }
    }
}
