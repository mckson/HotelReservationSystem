using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HotelReservation.API.Application.Commands.Hotel;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;

namespace HotelReservation.API.Application.Handlers.Hotel
{
    public class DeleteHotelHandler : IRequestHandler<DeleteHotelCommand, HotelResponseModel>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public DeleteHotelHandler(IHotelRepository hotelRepository, IMapper mapper, ILogger logger)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<HotelResponseModel> Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
        {
            _logger.Debug($"Hotel with {request.Id} is deleting");

            var deletedHotel = await _hotelRepository.DeleteAsync(request.Id) ??
                               throw new BusinessException($"Hotel with id {request.Id} does not exist", ErrorStatus.NotFound);

            var deletedHotelResponse = _mapper.Map<HotelResponseModel>(deletedHotel);

            _logger.Debug($"Hotel with {request.Id} was deleted");

            return deletedHotelResponse;
        }
    }
}
