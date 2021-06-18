using AutoMapper;
using HotelReservation.API.Models.RequestModels;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Constants;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesService _servicesService;
        private readonly IMapper _mapper;

        public ServicesController(
            IServicesService servicesService,
            IMapper mapper)
        {
            _servicesService = servicesService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<ServiceResponseModel>> GetAllServices()
        {
            var serviceModels = _servicesService.GetAllServices();
            var servicesResponseModels = _mapper.Map<IEnumerable<ServiceResponseModel>>(serviceModels);

            return Ok(servicesResponseModels);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ServiceResponseModel>> GetServiceAsync(int id)
        {
            var serviceModel = await _servicesService.GetAsync(id);
            var serviceResponseModel = _mapper.Map<ServiceResponseModel>(serviceModel);

            return Ok(serviceResponseModel);
        }

        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpPost]
        public async Task<ActionResult<ServiceResponseModel>> CreateServiceAsync([FromBody] ServiceRequestModel serviceRequestModel)
        {
            var userClaims = User.Claims;

            var serviceModel = _mapper.Map<ServiceModel>(serviceRequestModel);
            var createdServiceModel = await _servicesService.CreateAsync(serviceModel, userClaims);
            var createdServiceResponseModel = _mapper.Map<ServiceResponseModel>(createdServiceModel);

            return Ok(createdServiceResponseModel);
        }

        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ServiceResponseModel>> UpdateServiceAsync(int id, [FromBody] ServiceRequestModel serviceRequestModel)
        {
            var userClaims = User.Claims;

            var serviceModel = _mapper.Map<ServiceModel>(serviceRequestModel);
            var updatedServiceModel = await _servicesService.UpdateAsync(id, serviceModel, userClaims);
            var updatedServiceResponseModel = _mapper.Map<ServiceResponseModel>(updatedServiceModel);

            return Ok(updatedServiceResponseModel);
        }

        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ServiceResponseModel>> DeleteServiceAsync(int id)
        {
            var userClaims = User.Claims;

            var deletedServiceModel = await _servicesService.DeleteAsync(id, userClaims);
            var deletedServiceResponseModel = _mapper.Map<ServiceResponseModel>(deletedServiceModel);

            return Ok(deletedServiceResponseModel);
        }
    }
}
