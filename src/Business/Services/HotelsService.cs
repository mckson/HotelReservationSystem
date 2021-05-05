using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Business.Interfaces;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;

namespace HotelReservation.Business.Services
{
    public class HotelsService : IHotelsService
    {
        private readonly IMapper<HotelEntity, HotelResponseModel, HotelRequestModel> _mapper;

        private readonly IRepository<HotelEntity> _repo;

        private readonly IRepository<CompanyEntity> _companyRepo;

        private readonly IRepository<LocationEntity> _locationRepo;

        private readonly IMapper<RoomEntity, RoomResponseModel, RoomRequestModel> _roomMapper;

        private readonly IMapper<LocationEntity, LocationResponseModel, LocationRequestModel> _locationMapper;

        private readonly IMapper<CompanyEntity, CompanyResponseModel, CompanyRequestModel> _companyMapper;

        public HotelsService(
            IMapper<HotelEntity, HotelResponseModel, HotelRequestModel> mapper,
            IMapper<RoomEntity, RoomResponseModel, RoomRequestModel> roomMapper,
            IMapper<LocationEntity, LocationResponseModel, LocationRequestModel> locationMapper,
            IMapper<CompanyEntity, CompanyResponseModel, CompanyRequestModel> companyMapper,
            IRepository<HotelEntity> hotelRepository,
            IRepository<CompanyEntity> companyRepository,
            IRepository<LocationEntity> locationRepo)
        {
            _mapper = mapper;
            _roomMapper = roomMapper;
            _locationMapper = locationMapper;
            _companyMapper = companyMapper;
            _repo = hotelRepository;
            _companyRepo = companyRepository;
            _locationRepo = locationRepo;
        }

        public async Task<HotelResponseModel> CreateAsync(HotelRequestModel requestModel)
        {
            // var companyEntity =
            //     (await _companyRepo.FindAsync(c => c.Title == requestModel.CompanyName)).FirstOrDefault();
            // var locationEntity = (await _locationRepo.FindAsync(l =>
            //     l.BuildingNumber == requestModel.BuildingNumber &&
            //     l.City == requestModel.City &&
            //     l.Street == requestModel.Street &&
            //     l.Region == requestModel.Region &&
            //     l.Country == requestModel.Country))
            //     .FirstOrDefault();
            //  companyEntity ??= new CompanyEntity
            //  {
            //      Title = requestModel.CompanyName
            //  };
            //  locationEntity ??= new LocationEntity
            //  {
            //      Country = requestModel.Country,
            //      Region = requestModel.Region,
            //      City = requestModel.City,
            //      Street = requestModel.Street,
            //      BuildingNumber = requestModel.BuildingNumber
            //  };
            var unused = await _companyRepo.GetAsync(requestModel.CompanyId) ??
                                throw new DataException("Company with such id does not exist", ErrorStatus.NotFound);
            var locationEntity = await _locationRepo.GetAsync(requestModel.LocationId) ??
                                 throw new DataException("Location with such id does not exist", ErrorStatus.NotFound);

            if (locationEntity.Hotel != null)
                throw new DataException("That location already has linked hotel", ErrorStatus.HasLinkedEntity);

            var hotelEntity = new HotelEntity
            {
                CompanyId = requestModel.CompanyId,
                Name = requestModel.HotelName,
                LocationId = requestModel.LocationId
            };

            await _repo.CreateAsync(hotelEntity);

            return _mapper.EntityToResponse(hotelEntity);
        }

        public async Task<HotelResponseModel> GetAsync(int id)
        {
            var hotelEntity = await _repo.GetAsync(id);
            return _mapper.EntityToResponse(hotelEntity);
        }

        public async Task DeleteAsync(int id)
        {
            var unused = await _repo.GetAsync(id) ?? throw new DataException(
                "Hotel with such id does not exist", ErrorStatus.NotFound);

            await _repo.DeleteAsync(id);
        }

        public async Task<HotelResponseModel> UpdateAsync(int id, HotelRequestModel requestModel)
        {
            var hotelEntity = await _repo.GetAsync(id) ??
                              throw new DataException("Hotel with such id does not exist", ErrorStatus.NotFound);

            if (requestModel.CompanyId != hotelEntity.CompanyId)
            {
                var unused = await _companyRepo.GetAsync(requestModel.CompanyId) ??
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

                hotelEntity.CompanyId = requestModel.CompanyId;
            }

            if (requestModel.LocationId != hotelEntity.LocationId)
            {
                var newLocationEntity = await _locationRepo.GetAsync(requestModel.LocationId) ??
                                        throw new DataException("Location with such id does not exist", ErrorStatus.NotFound);

                if (newLocationEntity.Hotel != null)
                    throw new DataException("That location already has linked hotel", ErrorStatus.HasLinkedEntity);

                if (hotelEntity.LocationId != null)
                {
                    var unused = await _locationRepo.GetAsync(hotelEntity.LocationId.Value) ??
                                         throw new DataException("Location with such id does not exist", ErrorStatus.NotFound);
                }

                hotelEntity.LocationId = requestModel.LocationId;
            }

            hotelEntity.Name = requestModel.HotelName;

            await _repo.UpdateAsync(hotelEntity);

            return _mapper.EntityToResponse(hotelEntity);
        }

        public IEnumerable<HotelResponseModel> GetHotels()
        {
            var hotelEntities = _repo.GetAll();
            var hotelResponseModels = hotelEntities.Select(entity => _mapper.EntityToResponse(entity));
            return hotelResponseModels;
        }

        public async Task<IEnumerable<RoomResponseModel>> GetHotelRooms(int id)
        {
            var hotelEntity = await _repo.GetAsync(id) ?? throw new DataException(
                "Hotel with such id does not exist",
                ErrorStatus.NotFound);

            return hotelEntity.Rooms.Select(roomEntity => _roomMapper.EntityToResponse(roomEntity));
        }

        public async Task<LocationResponseModel> GetHotelLocation(int id)
        {
            var hotelEntity = await _repo.GetAsync(id) ?? throw new DataException(
                "Hotel with such id does not exist",
                ErrorStatus.NotFound);

            return _locationMapper.EntityToResponse(hotelEntity.Location);
        }

        public async Task<CompanyResponseModel> GetHotelCompany(int id)
        {
            var hotelEntity = await _repo.GetAsync(id) ?? throw new DataException(
                "Hotel with such id does not exist",
                ErrorStatus.NotFound);

            return _companyMapper.EntityToResponse(hotelEntity.Company);
        }
    }
}
