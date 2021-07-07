using HotelReservation.API.Commands.Service;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.API.Queries.Service;
using HotelReservation.Business;
using HotelReservation.Business.Constants;
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

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceResponseModel>>> GetAllServices()
        {
            var query = new GetAllServicesQuery();
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
