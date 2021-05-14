using System;
using AutoMapper;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelReservation.Business.Services
{
    public class ServicesService : IServicesService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<ServiceEntity> _serviceRepository;
        private readonly IHotelRepository _hotelRepository;

        public ServicesService(
            IRepository<ServiceEntity> serviceRepository,
            IHotelRepository hotelRepository,
            IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        public async Task<ServiceModel> CreateAsync(ServiceModel serviceModel, IEnumerable<Claim> userClaims)
        {
            var serviceEntity = _mapper.Map<ServiceEntity>(serviceModel);

            var hotelEntity = await _hotelRepository.GetAsync(serviceEntity.HotelId) ??
                              throw new BusinessException("No hotel with such id", ErrorStatus.NotFound);

            await CheckHotelManagementPermissionAsync(hotelEntity.Id, userClaims);

            if (hotelEntity.Services.Any(service => string.Equals(service.Name, serviceEntity.Name, StringComparison.CurrentCultureIgnoreCase)))
                throw new BusinessException($"Service with such name already exist in {hotelEntity.Name}", ErrorStatus.AlreadyExist);

            var createdServiceEntity = await _serviceRepository.CreateAsync(serviceEntity);
            var createdServiceModel = _mapper.Map<ServiceModel>(createdServiceEntity);

            return createdServiceModel;
        }

        public async Task<ServiceModel> GetAsync(int id)
        {
            var serviceEntity = await _serviceRepository.GetAsync(id) ??
                                throw new BusinessException(
                                    "Service with such id does not exist",
                                    ErrorStatus.NotFound);

            var serviceModel = _mapper.Map<ServiceModel>(serviceEntity);

            return serviceModel;
        }

        public async Task<ServiceModel> DeleteAsync(int id, IEnumerable<Claim> userClaims)
        {
            var serviceEntity = await _serviceRepository.GetAsync(id, true) ??
                                throw new BusinessException("No service with such id", ErrorStatus.NotFound);

            await CheckHotelManagementPermissionAsync(serviceEntity.HotelId, userClaims);

            var deletedServiceEntity = await _serviceRepository.DeleteAsync(id);
            var deletedServiceModel = _mapper.Map<ServiceModel>(deletedServiceEntity);

            return deletedServiceModel;
        }

        public async Task<ServiceModel> UpdateAsync(int id, ServiceModel updatingServiceModel, IEnumerable<Claim> userClaims)
        {
            var serviceEntity = await _serviceRepository.GetAsync(id, true) ??
                             throw new BusinessException("No service with such id", ErrorStatus.NotFound);

            var hotelEntity = await _hotelRepository.GetAsync(serviceEntity.HotelId) ??
                              throw new BusinessException("No hotel with such id", ErrorStatus.NotFound);

            await CheckHotelManagementPermissionAsync(hotelEntity.Id, userClaims);

            if (_serviceRepository.GetAll(true).Any(service =>
                string.Equals(service.Name, updatingServiceModel.Name, StringComparison.CurrentCultureIgnoreCase) &&
                service.HotelId == serviceEntity.HotelId &&
                serviceEntity.Id != service.Id))
            {
                throw new BusinessException(
                    $"Service with such name already exist in {hotelEntity.Name}",
                    ErrorStatus.AlreadyExist);
            }

            var updatingServiceEntity = _mapper.Map<ServiceEntity>(updatingServiceModel);
            updatingServiceEntity.Id = id;

            ServiceEntity updatedServiceEntity;
            try
            {
                updatedServiceEntity = await _serviceRepository.UpdateAsync(updatingServiceEntity);
            }
            catch
            {
                throw new BusinessException("Updating error! No service with such id", ErrorStatus.NotFound);
            }

            var updatedServiceModel = _mapper.Map<ServiceModel>(updatedServiceEntity);

            return updatedServiceModel;
        }

        public IEnumerable<ServiceModel> GetAllServices()
        {
            var serviceEntities = _serviceRepository.GetAll() ??
                                  throw new BusinessException("No services were created", ErrorStatus.NotFound);

            var serviceModels = _mapper.Map<IEnumerable<ServiceModel>>(serviceEntities);

            return serviceModels;
        }

        private async Task CheckHotelManagementPermissionAsync(int id, IEnumerable<Claim> userClaims)
        {
            var claims = userClaims.ToList();
            if (claims.Where(claim => claim.Type.Equals(ClaimTypes.Role)).Any(role => role.Value.ToUpper() == "ADMIN"))
                return;

            var hotelEntity = await _hotelRepository.GetAsync(id, true) ??
                              throw new BusinessException("No hotel with such id", ErrorStatus.NotFound);

            var hotelIdString = claims.FirstOrDefault(claim => claim.Type == "hotelId")?.Value;
            int.TryParse(hotelIdString, out var hotelId);

            if (hotelId == 0)
            {
                throw new BusinessException(
                    "You have no permissions to manage hotels. Ask application admin to take that permission",
                    ErrorStatus.AccessDenied);
            }

            if (hotelId != hotelEntity.Id)
            {
                throw new BusinessException(
                    $"You have no permission to manage hotel {hotelEntity.Name}. Ask application admin about permissions",
                    ErrorStatus.AccessDenied);
            }
        }
    }
}
