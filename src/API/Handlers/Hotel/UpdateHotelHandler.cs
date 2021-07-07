using AutoMapper;
using HotelReservation.API.Commands.Hotel;
using HotelReservation.API.Models.RequestModels;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Handlers.Hotel
{
    public class UpdateHotelHandler : IRequestHandler<UpdateHotelCommand, HotelResponseModel>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UpdateHotelHandler(IHotelRepository hotelRepository, ILocationRepository locationRepository, IMapper mapper, ILogger logger)
        {
            _hotelRepository = hotelRepository;
            _locationRepository = locationRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<HotelResponseModel> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
        {
            _logger.Debug($"Hotel with {request.Id} is updating");

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

            if (!IsLocationEqual(hotelEntity.Location, request.Location))
            {
                await UpdateLocationEntityFieldsAsync(hotelEntity.Location, request.Location);
            }

            var updatedHotel = await _hotelRepository.UpdateAsync(hotelEntity);
            var updatedHotelResponse = _mapper.Map<HotelResponseModel>(updatedHotel);

            _logger.Debug($"Hotel with {request.Id} was updated");

            return updatedHotelResponse;
        }

        private bool IsLocationEqual(LocationEntity locationOne, LocationRequestModel locationTwo)
        {
            return locationOne.Country.Equals(locationTwo.Country, StringComparison.InvariantCultureIgnoreCase) &&
                   locationOne.Region.Equals(locationTwo.Region, StringComparison.InvariantCultureIgnoreCase) &&
                   locationOne.City.Equals(locationTwo.City, StringComparison.InvariantCultureIgnoreCase) &&
                   locationOne.Street.Equals(locationTwo.Street, StringComparison.InvariantCultureIgnoreCase) &&
                   locationOne.BuildingNumber == locationTwo.BuildingNumber;
        }

        private async Task UpdateLocationEntityFieldsAsync(LocationEntity locationToUpdate, LocationRequestModel locationModel)
        {
            var existingLocation = await _locationRepository.GetAsync(
                locationModel.Country,
                locationModel.Region,
                locationModel.City,
                locationModel.Street,
                locationModel.BuildingNumber);

            if (existingLocation != null)
                throw new BusinessException("Such location already owned", ErrorStatus.AlreadyExist);

            locationToUpdate.Country = locationModel.Country;
            locationToUpdate.Region = locationModel.Region;
            locationToUpdate.City = locationModel.City;
            locationToUpdate.Street = locationModel.Street;
            locationToUpdate.BuildingNumber = locationModel.BuildingNumber;
        }
    }
}
