using AutoMapper;
using Castle.Core.Internal;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Filters;
using HotelReservation.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System;
using System.Collections.Generic;
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
        private readonly IRepository<ImageEntity> _imageRepository;
        private readonly UserManager<UserEntity> _userManager;

        public HotelsService(
            IMapper mapper,
            IHotelRepository hotelRepository,
            ILocationRepository locationRepository,
            IRepository<ImageEntity> imageRepository,
            UserManager<UserEntity> userManager,
            ILogger logger)
        {
            _hotelRepository = hotelRepository;
            _locationRepository = locationRepository;
            _imageRepository = imageRepository;
            _mapper = mapper;
            _userManager = userManager;
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

            var hotelModel = _mapper.Map<HotelModel>(hotelEntity);

            foreach (var hotelUser in hotelModel.HotelUsers)
            {
                await GetRolesForUserModelAsync(hotelUser.User);
            }

            _logger.Debug($"Hotel with {id} was get");

            return hotelModel;
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

            // if (hotelEntity.MainImage != null)
            // {
            //     await _imageRepository.DeleteAsync(hotelEntity.MainImage.Id);
            // }
            var newMainImage = _mapper.Map<ImageEntity>(updatingHotelModel.MainImage);

            hotelEntity.Images = _mapper.Map<ICollection<ImageEntity>>(updatingHotelModel.Images);

            await ChangeHotelMainImageAsync(hotelEntity, newMainImage);

            if (!IsLocationEqual(_mapper.Map<LocationModel>(hotelEntity.Location), updatingHotelModel.Location))
            {
                await UpdateLocationEntityFieldsAsync(hotelEntity.Location, updatingHotelModel.Location);
            }

            var updatedHotel = await _hotelRepository.UpdateAsync(hotelEntity);

            _logger.Debug($"Hotel with {id} was updated");

            return _mapper.Map<HotelModel>(updatedHotel);
        }

        public async Task<IEnumerable<HotelModel>> GetAllHotelsAsync()
        {
            _logger.Debug("Hotels are requesting");

            var hotelEntities = _hotelRepository.GetAll();
            var hotelModels = _mapper.Map<IEnumerable<HotelModel>>(hotelEntities);

            foreach (var hotelModel in hotelModels)
            {
                foreach (var hotelUser in hotelModel.HotelUsers)
                {
                    await GetRolesForUserModelAsync(hotelUser.User);
                }
            }

            _logger.Debug("Hotels are requested");
            return hotelModels;
        }

        public async Task<IEnumerable<HotelModel>> GetPagedHotelsAsync(PaginationFilter paginationFilter, HotelsFilter filter)
        {
            _logger.Debug($"Paged hotels are requesting. Page: {paginationFilter.PageNumber}, Size: {paginationFilter.PageSize}");

            var hotelEntities = _hotelRepository.Find(
                FilterExpression(filter),
                paginationFilter);
            var hotelModels = _mapper.Map<IEnumerable<HotelModel>>(hotelEntities);

            foreach (var hotelModel in hotelModels)
            {
                foreach (var hotelUser in hotelModel.HotelUsers)
                {
                    await GetRolesForUserModelAsync(hotelUser.User);
                }
            }

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
                ((!filter.DateIn.HasValue || !filter.DateOut.HasValue) || hotel.Rooms.Any(room =>
                    !room.ReservationRooms.Any(rr =>
                        (rr.Reservation.DateIn >= filter.DateIn && rr.Reservation.DateIn < filter.DateOut) ||
                        (rr.Reservation.DateOut > filter.DateIn && rr.Reservation.DateOut <= filter.DateOut)))) &&
                (!filter.ManagerId.HasValue || hotel.HotelUsers.Any(hu => hu.UserId == filter.ManagerId.Value)) &&
                (filter.Name.IsNullOrEmpty() || hotel.Name.StartsWith(filter.Name)) &&
                (filter.City.IsNullOrEmpty() || hotel.Location.City.StartsWith(filter.City)) &&
                (filter.Services.IsNullOrEmpty() || hotel.Services.Any(service =>
                    filter.Services.First().IsNullOrEmpty() || service.Name.StartsWith(filter.Services.First())));
        }

        private async Task GetRolesForUserModelAsync(UserModel userModel)
        {
            _logger.Debug($"Manager {userModel.Id} roles are requesting");

            var roles = await _userManager.GetRolesAsync(_mapper.Map<UserEntity>(userModel));
            userModel.Roles = roles;

            _logger.Debug($"Manager {userModel.Id} roles requested");
        }

        private async Task ChangeHotelMainImageAsync(HotelEntity hotelEntity, ImageEntity newImage)
        {
            _logger.Debug($"Main image of hotel {hotelEntity.Id} is updating");

            var oldImage = _imageRepository.Find(image => image.IsMain && image.HotelId == hotelEntity.Id).FirstOrDefault();

            if (oldImage != null && newImage != null)
            {
                oldImage.Name = newImage.Name;
                oldImage.Type = newImage.Type;
                oldImage.Image = newImage.Image;

                // await _imageRepository.UpdateAsync(oldImage);
            }
            else
            {
                if (newImage != null)
                {
                    newImage.IsMain = true;
                    hotelEntity.Images.Add(newImage);
                    // await _imageRepository.CreateAsync(newImage);
                }
                else if (oldImage != null)
                {
                    hotelEntity.Images.Remove(oldImage);
                    // await _imageRepository.DeleteAsync(oldImage.Id);
                }
            }

            _logger.Debug($"Main image of hotel {hotelEntity.Id} is updated");
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
