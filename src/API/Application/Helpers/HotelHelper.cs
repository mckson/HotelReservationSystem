using HotelReservation.API.Application.Interfaces;
using HotelReservation.API.Models.RequestModels;
using HotelReservation.Business;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using System;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Helpers
{
    public class HotelHelper : IHotelHelper
    {
        private readonly ILocationRepository _locationRepository;

        public HotelHelper(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public bool IsLocationEqual(LocationEntity locationOne, LocationRequestModel locationTwo)
        {
            return locationOne.Country.Equals(locationTwo.Country, StringComparison.InvariantCultureIgnoreCase) &&
                   locationOne.Region.Equals(locationTwo.Region, StringComparison.InvariantCultureIgnoreCase) &&
                   locationOne.City.Equals(locationTwo.City, StringComparison.InvariantCultureIgnoreCase) &&
                   locationOne.Street.Equals(locationTwo.Street, StringComparison.InvariantCultureIgnoreCase) &&
                   locationOne.BuildingNumber == locationTwo.BuildingNumber;
        }

        public async Task UpdateLocationEntityFieldsAsync(LocationEntity locationToUpdate, LocationRequestModel locationModel)
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
