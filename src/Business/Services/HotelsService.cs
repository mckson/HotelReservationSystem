using AutoMapper;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelReservation.Business.Services
{
    public class HotelsService : IHotelsService
    {
        private readonly IHotelRepository _repo;

        private readonly ILocationRepository _locationRepo;

        private readonly IMapper _mapper;

        public HotelsService(
            IMapper mapper,
            IHotelRepository hotelRepository,
            ILocationRepository locationRepo)
        {
            _repo = hotelRepository;
            _locationRepo = locationRepo;
            _mapper = mapper;
        }

        public async Task<HotelModel> CreateAsync(HotelModel userModel, IEnumerable<Claim> userClaims)
        {
            var locationEntity = await _locationRepo.GetAsync(
                userModel.Location.Country,
                userModel.Location.Region,
                userModel.Location.City,
                userModel.Location.Street,
                userModel.Location.BuildingNumber);

            if (locationEntity != null)
                throw new BusinessException("Location already exist", ErrorStatus.AlreadyExist);

            locationEntity = _mapper.Map<LocationEntity>(userModel.Location);
            // var createdLocation = await _locationRepo.CreateAsync(locationEntity);
            var hotelEntity = _mapper.Map<HotelEntity>(userModel);

            hotelEntity.Location = locationEntity;

            var createdHotelModel = await _repo.CreateAsync(hotelEntity);

            return _mapper.Map<HotelModel>(createdHotelModel);
        }

        public async Task<HotelModel> GetAsync(int id)
        {
            var hotelEntity = await _repo.GetAsync(id);
            return _mapper.Map<HotelModel>(hotelEntity);
        }

        public async Task<HotelModel> DeleteAsync(int id, IEnumerable<Claim> userClaims)
        {
            var unused = await _repo.GetAsync(id) ?? throw new BusinessException(
                "Hotel with such id does not exist", ErrorStatus.NotFound);

            var deletedHotel = await _repo.DeleteAsync(id);

            return _mapper.Map<HotelModel>(deletedHotel);
        }

        public async Task<HotelModel> UpdateAsync(int id, HotelModel updatingRoomModel, IEnumerable<Claim> userClaims)
        {
            var hotelEntity = await _repo.GetAsync(id) ??
                              throw new BusinessException("Hotel with such id does not exist", ErrorStatus.NotFound);

            hotelEntity.Name = updatingRoomModel.Name;

            var updatedHotel = await _repo.UpdateAsync(hotelEntity);
            return _mapper.Map<HotelModel>(updatedHotel);
        }

        public IEnumerable<HotelModel> GetHotels()
        {
            var hotelEntities = _repo.GetAll();
            var hotelModels = _mapper.Map<IEnumerable<HotelModel>>(hotelEntities);
            return hotelModels;
        }

        // public async Task<IEnumerable<HotelModel>> GetHotelRooms(int id)
        // {
        //     var hotelEntity = await _repo.GetAsync(id) ?? throw new DataException(
        //         "Hotel with such id does not exist",
        //         ErrorStatus.NotFound);
        //     return hotelEntity.Rooms.Select(roomEntity => _roomMapper.EntityToResponse(roomEntity));
        // }
        // public async Task<LocationResponseModel> GetHotelLocation(int id)
        // {
        //     var hotelEntity = await _repo.GetAsync(id) ?? throw new DataException(
        //         "Hotel with such id does not exist",
        //         ErrorStatus.NotFound);
        //     return _locationMapper.EntityToResponse(hotelEntity.Location);
        // }
        // public async Task<CompanyResponseModel> GetHotelCompany(int id)
        // {
        //     var hotelEntity = await _repo.GetAsync(id) ?? throw new DataException(
        //         "Hotel with such id does not exist",
        //         ErrorStatus.NotFound);
        //     return _companyMapper.EntityToResponse(hotelEntity.Company);
        // }
    }
}
