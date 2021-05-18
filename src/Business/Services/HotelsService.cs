using AutoMapper;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Serilog;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelReservation.Business.Services
{
    public class HotelsService : IHotelsService
    {
        private readonly IHotelRepository _hotelRepository;

        private readonly ILocationRepository _locationRepository;

        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public HotelsService(
            IMapper mapper,
            IHotelRepository hotelRepository,
            ILocationRepository locationRepository,
            ILogger logger)
        {
            _hotelRepository = hotelRepository;
            _locationRepository = locationRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<HotelModel> CreateAsync(HotelModel hotelModel, IEnumerable<Claim> userClaims)
        {
            _logger.Debug($"Hotel {hotelModel.Name} is creating");

            var locationEntity = await _locationRepository.GetAsync(
                hotelModel.Location.Country,
                hotelModel.Location.Region,
                hotelModel.Location.City,
                hotelModel.Location.Street,
                hotelModel.Location.BuildingNumber);

            if (locationEntity != null)
                throw new BusinessException("Location already exist", ErrorStatus.AlreadyExist);

            locationEntity = _mapper.Map<LocationEntity>(hotelModel.Location);
            var hotelEntity = _mapper.Map<HotelEntity>(hotelModel);

            hotelEntity.Location = locationEntity;

            var createdHotelModel = await _hotelRepository.CreateAsync(hotelEntity);

            _logger.Debug($"Hotel {hotelModel.Name} was created");

            return _mapper.Map<HotelModel>(createdHotelModel);
        }

        public async Task<HotelModel> GetAsync(int id)
        {
            _logger.Debug($"Hotel with {id} is getting");

            var hotelEntity = await _hotelRepository.GetAsync(id) ??
                              throw new BusinessException($"Hotel with {id} does not exist", ErrorStatus.NotFound);

            _logger.Debug($"Hotel with {id} was get");

            return _mapper.Map<HotelModel>(hotelEntity);
        }

        public async Task<HotelModel> DeleteAsync(int id, IEnumerable<Claim> userClaims)
        {
            _logger.Debug($"Hotel with {id} is deleting");

            var deletedHotel = await _hotelRepository.DeleteAsync(id) ??
                               throw new BusinessException($"Hotel with id {id} does not exist", ErrorStatus.NotFound);

            _logger.Debug($"Hotel with {id} was deleted");

            return _mapper.Map<HotelModel>(deletedHotel);
        }

        public async Task<HotelModel> UpdateAsync(int id, HotelModel updatingHotelModel, IEnumerable<Claim> userClaims)
        {
            _logger.Debug($"Hotel with {id} is updating");

            var hotelEntity = await _hotelRepository.GetAsync(id) ??
                              throw new BusinessException("Hotel with such id does not exist", ErrorStatus.NotFound);

            hotelEntity.Name = updatingHotelModel.Name;
            var updatedHotel = await _hotelRepository.UpdateAsync(hotelEntity);

            _logger.Debug($"Hotel with {id} was updated");

            return _mapper.Map<HotelModel>(updatedHotel);
        }

        public IEnumerable<HotelModel> GetHotels()
        {
            _logger.Debug("Hotels are requesting");

            var hotelEntities = _hotelRepository.GetAll();
            var hotelModels = _mapper.Map<IEnumerable<HotelModel>>(hotelEntities);

            _logger.Debug("Hotels are requested");
            return hotelModels;
        }

        public async Task<HotelModel> GetHotelByNameAsync(string name)
        {
            var hotelEntity = await _hotelRepository.GetAsync(name) ??
                             throw new BusinessException($"There is no hotel with name {name}", ErrorStatus.NotFound);

            var hotelModel = _mapper.Map<HotelModel>(hotelEntity);

            return hotelModel;
        }
    }
}
