﻿using AutoMapper;
using Castle.Core.Internal;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Filters;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore.Internal;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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
            hotelEntity.Deposit = updatingHotelModel.Deposit;
            hotelEntity.NumberFloors = updatingHotelModel.NumberFloors;
            hotelEntity.HotelUsers = _mapper.Map<IEnumerable<HotelUserEntity>>(updatingHotelModel.HotelUsers);
            hotelEntity.Description = updatingHotelModel.Description;
            hotelEntity.MainImage = _mapper.Map<MainImageEntity>(updatingHotelModel.MainImage);
            hotelEntity.Images = _mapper.Map<IEnumerable<ImageEntity>>(updatingHotelModel.Images);

            if (!IsLocationEqual(_mapper.Map<LocationModel>(hotelEntity.Location), updatingHotelModel.Location))
            {
                await UpdateLocationEntityFieldsAsync(hotelEntity.Location, updatingHotelModel.Location);
            }

            var updatedHotel = await _hotelRepository.UpdateAsync(hotelEntity);

            _logger.Debug($"Hotel with {id} was updated");

            return _mapper.Map<HotelModel>(updatedHotel);
        }

        public IEnumerable<HotelModel> GetAllHotels()
        {
            _logger.Debug("Hotels are requesting");

            var hotelEntities = _hotelRepository.GetAll();
            var hotelModels = _mapper.Map<IEnumerable<HotelModel>>(hotelEntities);

            _logger.Debug("Hotels are requested");
            return hotelModels;
        }

        public IEnumerable<HotelModel> GetPagedHotels(PaginationFilter paginationFilter, HotelsFilter filter)
        {
            _logger.Debug($"Paged hotels are requesting. Page: {paginationFilter.PageNumber}, Size: {paginationFilter.PageSize}");

            var hotelEntities = _hotelRepository.Find(
                FilterExpression(filter),
                paginationFilter);
            var hotelModels = _mapper.Map<IEnumerable<HotelModel>>(hotelEntities);

            _logger.Debug($"Paged hotels are requested. Page: {paginationFilter.PageNumber}, Size: {paginationFilter.PageSize}");
            return hotelModels;
        }

        public async Task<int> GetCountAsync(HotelsFilter filter)
        {
            _logger.Debug("Hotels count are requesting");

            var count = await _hotelRepository.GetCountAsync(FilterExpression(filter));

            _logger.Debug("Hotels count are requested");
            return count;
        }

        public async Task<HotelModel> GetHotelByNameAsync(string name)
        {
            var hotelEntity = await _hotelRepository.GetAsync(name) ??
                             throw new BusinessException($"There is no hotel with name {name}", ErrorStatus.NotFound);

            var hotelModel = _mapper.Map<HotelModel>(hotelEntity);

            return hotelModel;
        }

        private bool IsLocationEqual(LocationModel locationOne, LocationModel locationTwo)
        {
            return locationOne.Country.Equals(locationTwo.Country, StringComparison.InvariantCultureIgnoreCase) &&
                   locationOne.Region.Equals(locationTwo.Region, StringComparison.InvariantCultureIgnoreCase) &&
                   locationOne.City.Equals(locationTwo.City, StringComparison.InvariantCultureIgnoreCase) &&
                   locationOne.Street.Equals(locationTwo.Street, StringComparison.InvariantCultureIgnoreCase) &&
                   locationOne.BuildingNumber == locationTwo.BuildingNumber;
        }

        private async Task UpdateLocationEntityFieldsAsync(LocationEntity locationToUpdate, LocationModel locationModel)
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

        private Expression<Func<HotelEntity, bool>> FilterExpression(HotelsFilter filter)
        {
            return hotel =>
                (filter.Name.IsNullOrEmpty() || hotel.Name.StartsWith(filter.Name)) &&
                (filter.City.IsNullOrEmpty() || hotel.Location.City.StartsWith(filter.City)) &&
                (filter.Services.IsNullOrEmpty() || hotel.Services.Any(service => filter.Services.First().IsNullOrEmpty() || service.Name.StartsWith(filter.Services.First())));
        }

        // private Expression<Func<HotelEntity, bool>> FilterHotelExpression(HotelsFilter filter)
        // {
        //     // // var hotelArgument = Expression.Parameter(typeof(HotelEntity), "hotel");
        //     // // var result = Expression.Variable(typeof(bool), "result");
        //     // Expression<Func<HotelEntity, bool>> isStartedName = hotel => filter.Name.IsNullOrEmpty() || hotel.Name.StartsWith(filter.Name);
        //     // Expression<Func<HotelEntity, bool>> isStartedCity = hotel =>
        //     //     filter.City.IsNullOrEmpty() || hotel.Location.City.StartsWith(filter.City);
        //     // Func<HotelEntity, bool> isContainServices = hotel =>
        //     //     filter.Services.IsNullOrEmpty() || filter.Services.Any(serv =>
        //     //         hotel.Services.Any(service => service.Name.StartsWith(serv)));
        //     // Func<HotelEntity, bool> isFilter = hotel => isStartedName(hotel) && isContainServices(hotel) && isStartedCity(hotel);
        //     var predicate = PredicateBuilder.True<HotelEntity>();
        //     predicate.And(hotel => filter.Name.IsNullOrEmpty() || hotel.Name.StartsWith(filter.Name));
        //     predicate.And(hotel => filter.City.IsNullOrEmpty() || hotel.Location.City.StartsWith(filter.City));
        //     predicate.And(hotel =>
        //         filter.Services.IsNullOrEmpty() || filter.Services.Any(serv =>
        //             hotel.Services.Any(service => service.Name.StartsWith(serv))));
        //     return predicate;
        // }
    }
}
