using AutoMapper;
using HotelReservation.API.Application.Commands.Hotel;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Hotel
{
    public class CreateHotelHandler : IRequestHandler<CreateHotelCommand, HotelResponseModel>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public CreateHotelHandler(
            IHotelRepository hotelRepository,
            IMapper mapper,
            ILocationRepository locationRepository)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
            _locationRepository = locationRepository;
        }

        public async Task<HotelResponseModel> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
        {
            var locationEntity = await _locationRepository.GetAsync(
                request.Location.Country,
                request.Location.Region,
                request.Location.City,
                request.Location.Street,
                request.Location.BuildingNumber);

            if (locationEntity != null)
                throw new BusinessException("Location already exist", ErrorStatus.AlreadyExist);

            locationEntity = _mapper.Map<LocationEntity>(request.Location);
            var hotelEntity = _mapper.Map<HotelEntity>(request);

            hotelEntity.Location = locationEntity;

            hotelEntity.HotelUsers = new List<HotelUserEntity>();

            if (request.Managers != null)
            {
                hotelEntity.HotelUsers.AddRange(request.Managers
                    .Select(manager => new HotelUserEntity { UserId = manager }).ToList());
            }

            var createdHotelEntity = await _hotelRepository.CreateAsync(hotelEntity);
            var createdHotelResponse = _mapper.Map<HotelResponseModel>(createdHotelEntity);

            return createdHotelResponse;
        }
    }
}
