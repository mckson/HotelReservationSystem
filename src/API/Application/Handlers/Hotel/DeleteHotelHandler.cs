using AutoMapper;
using HotelReservation.API.Application.Commands.Hotel;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Hotel
{
    public class DeleteHotelHandler : IRequestHandler<DeleteHotelCommand, HotelResponseModel>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;

        public DeleteHotelHandler(
            IHotelRepository hotelRepository,
            IMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        public async Task<HotelResponseModel> Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
        {

            var deletedHotel = await _hotelRepository.DeleteAsync(request.Id) ??
                               throw new BusinessException($"Hotel with id {request.Id} does not exist", ErrorStatus.NotFound);

            var deletedHotelResponse = _mapper.Map<HotelResponseModel>(deletedHotel);

            return deletedHotelResponse;
        }
    }
}
