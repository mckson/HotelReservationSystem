using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Mappers;
using HotelReservation.Business.Models.RequestModels;
using HotelReservation.Business.Models.ResponseModels;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Repositories;

namespace HotelReservation.Business.Services
{
    public class HotelsService : IHotelsService
    {
        private readonly HotelMapper _mapper;
        private readonly HotelRepository _repo;
        private readonly CompanyRepository _companyRepo;
        private readonly LocationRepository _locationRepo;

        public HotelsService(HotelMapper mapper, HotelRepository hotelRepository, CompanyRepository companyRepository, LocationRepository locationRepo)
        {
            _mapper = mapper;
            _repo = hotelRepository;
            _companyRepo = companyRepository;
            _locationRepo = locationRepo;
        }

        public async Task<HotelResponseModel> CreateAsync(HotelRequestModel requestModel)
        {
            var companyEntity =
                (await _companyRepo.FindAsync(c => c.Title == requestModel.CompanyName)).FirstOrDefault();

            var locationEntity = (await _locationRepo.FindAsync(l =>
                l.BuildingNumber == requestModel.BuildingNumber &&
                l.City == requestModel.City &&
                l.Street == requestModel.Street &&
                l.Region == requestModel.Region &&
                l.Country == requestModel.Country
            )).FirstOrDefault();

            companyEntity ??= new CompanyEntity
            {
                Title = requestModel.CompanyName
            };

            locationEntity ??= new LocationEntity
            {
                Country = requestModel.Country,
                Region = requestModel.Region,
                City = requestModel.City,
                Street = requestModel.Street,
                BuildingNumber = requestModel.BuildingNumber
            };

            var hotelEntity = new HotelEntity
            {
                Company = companyEntity,
                CompanyId = companyEntity.Id,
                Name = requestModel.HotelName,
                Location = locationEntity
            };

            await _repo.CreateAsync(hotelEntity);

            return _mapper.EntityToResponse(hotelEntity);
        }

        public async Task<HotelResponseModel> GetAsync(int id)
        {
            var hotelEntity = await _repo.GetAsync(id);
            return _mapper.EntityToResponse(hotelEntity);
        }

        public Task DeleteAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<HotelResponseModel> UpdateAsync(HotelRequestModel updateModel)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<HotelResponseModel>> GetHotelsAsync()
        {
            var hotelEntities = await _repo.GetAllAsync();
            var hotelResponseModels = hotelEntities.Select(entity => _mapper.EntityToResponse(entity));
            return hotelResponseModels;
        }
    }
}
