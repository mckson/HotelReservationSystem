using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;

namespace HotelReservation.Business.Services
{
    public class HotelsService : IHotelsService
    {
        private readonly IRepository<HotelEntity> _repo;

        private readonly IRepository<CompanyEntity> _companyRepo;

        private readonly IRepository<LocationEntity> _locationRepo;
        private readonly IMapper _mapper;

        public HotelsService(
            IMapper mapper,
            IRepository<HotelEntity> hotelRepository,
            IRepository<CompanyEntity> companyRepository,
            IRepository<LocationEntity> locationRepo)
        {
            _repo = hotelRepository;
            _companyRepo = companyRepository;
            _locationRepo = locationRepo;
            _mapper = mapper;
        }

        public async Task<HotelModel> CreateAsync(HotelModel model)
        {
            var unused = await _companyRepo.GetAsync(model.Company.Title) ??
                                throw new DataException("Company with such id does not exist", ErrorStatus.NotFound);
            var locationEntity = await _locationRepo.GetAsync(model.LocationId.Value) ??
                                 throw new DataException("Location with such id does not exist", ErrorStatus.NotFound);

            if (locationEntity.Hotel != null)
                throw new DataException("That location already has linked hotel", ErrorStatus.HasLinkedEntity);

            // var hotelEntity = new HotelEntity
            // {
            //     CompanyId = model.CompanyId,
            //     Name = model.Name,
            //     LocationId = model.LocationId
            // };
            var hotelEntity = _mapper.Map<HotelEntity>(model);

            await _repo.CreateAsync(hotelEntity);

            return model;
        }

        public async Task<HotelModel> GetAsync(int id)
        {
            var hotelEntity = await _repo.GetAsync(id);
            return _mapper.Map<HotelModel>(hotelEntity);
        }

        public async Task<HotelModel> DeleteAsync(int id)
        {
            var unused = await _repo.GetAsync(id) ?? throw new DataException(
                "Hotel with such id does not exist", ErrorStatus.NotFound);

            var deletedHotel = await _repo.DeleteAsync(id);

            return _mapper.Map<HotelModel>(deletedHotel);
        }

        public async Task<HotelModel> UpdateAsync(int id, HotelModel model)
        {
            // var hotelEntity = _mapper.Map<HotelEntity>(model);
            var hotelEntity = await _repo.GetAsync(id) ??
                              throw new DataException("Hotel with such id does not exist", ErrorStatus.NotFound);

            if (model.CompanyId != hotelEntity.CompanyId)
            {
                var unused = await _companyRepo.GetAsync(model.CompanyId.Value) ??
                                       throw new DataException(
                                           "Company with such id does not exist",
                                           ErrorStatus.NotFound);

                if (hotelEntity.CompanyId != null)
                {
                    var unused1 = await _companyRepo.GetAsync(hotelEntity.CompanyId.Value) ??
                                       throw new DataException(
                                           "Company with such id does not exist",
                                           ErrorStatus.NotFound);
                }

                hotelEntity.CompanyId = model.CompanyId;
            }

            if (model.LocationId != hotelEntity.LocationId)
            {
                var newLocationEntity = await _locationRepo.GetAsync(model.LocationId.Value) ??
                                        throw new DataException("Location with such id does not exist", ErrorStatus.NotFound);

                if (newLocationEntity.Hotel != null)
                    throw new DataException("That location already has linked hotel", ErrorStatus.HasLinkedEntity);

                if (hotelEntity.LocationId != null)
                {
                    var unused = await _locationRepo.GetAsync(hotelEntity.LocationId.Value) ??
                                         throw new DataException("Location with such id does not exist", ErrorStatus.NotFound);
                }

                hotelEntity.LocationId = model.LocationId;
            }

            await _repo.UpdateAsync(hotelEntity);

            return model;
        }

        public IEnumerable<HotelModel> GetHotels()
        {
            var hotelEntities = _repo.GetAll();
            var hotelResponseModels = _mapper.Map<IEnumerable<HotelModel>>(hotelEntities);
            return hotelResponseModels;
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
