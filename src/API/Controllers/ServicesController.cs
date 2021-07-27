using HotelReservation.API.Application.Commands.Service;
using HotelReservation.API.Application.Queries.Service;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Business.Constants;
using HotelReservation.Data.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ServicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Method that allow retrieve a page of service models
        /// </summary>
        /// <param name="paginationFilter">Filter for pages (page's number, page's size)</param>
        /// <param name="servicesFilter">Filter over services (hotel's id, service's name etc)</param>
        /// <returns>BasePagedResponseModel with ServiceResponseModels as content</returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<BasePagedResponseModel<ServiceResponseModel>>> GetPagedFilteredServicesAsync(
            [FromQuery] PaginationFilter paginationFilter,
            [FromQuery] ServicesFilter servicesFilter)
        {
            var query = new GetPagedFilteredServicesQuery
            {
                PaginationFilter = paginationFilter,
                ServicesFilter = servicesFilter
            };
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<ServicePromptResponseModel>>> GetServiceSearchVariantsAsync(
            [FromQuery] ServicesFilter servicesFilter)
        {
            var query = new GetServiceSearchVariantsQuery
            {
                ServicesFilter = servicesFilter
            };

            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ServiceResponseModel>> GetServiceAsync(Guid id)
        {
            var query = new GetServiceByIdQuery { Id = id };
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpPost]
        public async Task<ActionResult<ServiceResponseModel>> CreateServiceAsync([FromBody] CreateServiceCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ServiceResponseModel>> UpdateServiceAsync(Guid id, [FromBody] UpdateServiceCommand command)
        {
            if (!id.Equals(command.Id))
            {
                throw new BusinessException(
                    "Updating resource id does not match with requested id",
                    ErrorStatus.IncorrectInput);
            }

            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ServiceResponseModel>> DeleteServiceAsync(Guid id)
        {
            var command = new DeleteServiceCommand { Id = id };
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
