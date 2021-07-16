using AutoMapper;
using HotelReservation.API.Application.Commands.Hotel;
using HotelReservation.API.Application.Interfaces;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Hotel
{
    public class UpdateHotelHandler : IRequestHandler<UpdateHotelCommand, HotelResponseModel>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IHotelHelper _hotelHelper;
        private readonly IMapper _mapper;

        public UpdateHotelHandler(
            IHotelRepository hotelRepository,
            IMapper mapper,
            IHotelHelper hotelHelper)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
            _hotelHelper = hotelHelper;
        }

        public async Task<HotelResponseModel> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
        {
            var hotelEntity = await _hotelRepository.GetAsync(request.Id) ??
                              throw new BusinessException("Hotel with such id does not exist", ErrorStatus.NotFound);

            hotelEntity.Name = request.Name;
            hotelEntity.Deposit = request.Deposit;
            hotelEntity.NumberFloors = request.NumberFloors;
            hotelEntity.HotelUsers.RemoveAll(hu => hu.HotelId == request.Id);

            if (request.Managers != null)
            {
                hotelEntity.HotelUsers.AddRange(request.Managers
                    .Select(manager => new HotelUserEntity { UserId = manager }).ToList());
            }

            hotelEntity.Description = request.Description;

            if (!_hotelHelper.IsLocationEqual(hotelEntity.Location, request.Location))
            {
                await _hotelHelper.TryUpdateLocationEntityFieldsAsync(hotelEntity.Location, request.Location);
            }

            var updatedHotel = await _hotelRepository.UpdateAsync(hotelEntity);
            var updatedHotelResponse = _mapper.Map<HotelResponseModel>(updatedHotel);

            return updatedHotelResponse;
        }
    }
}
