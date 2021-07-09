using HotelReservation.API.Application.Commands.Hotel;
using HotelReservation.API.Application.Queries.Hotel;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Business.Constants;
using HotelReservation.Data.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HotelReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HotelsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<BasePagedResponseModel<HotelResponseModel>> GetHotelsAsync([FromQuery] PaginationFilter paginationFilter, [FromQuery] HotelsFilter hotelsFilter)
        {
            var route = Request.Path.Value;

            var query = new GetPagedFilteredHotelsQuery
            {
                PaginationFilter = paginationFilter,
                HotelsFilter = hotelsFilter,
                Route = route
            };

            var response = await _mediator.Send(query);

            return response;
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<HotelResponseModel>> GetHotelByIdAsync(Guid id)
        {
            var query = new GetHotelByIdQuery { Id = id };
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [Authorize(Policy = Policies.AdminPermission)]
        [HttpPost]
        public async Task<ActionResult<HotelResponseModel>> CreateHotelAsync([FromBody] CreateHotelCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = Policies.AdminPermission)]
        public async Task<ActionResult<HotelResponseModel>> UpdateHotelAsync(Guid id, [FromBody] UpdateHotelCommand command)
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

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = Policies.AdminPermission)]
        public async Task<ActionResult<HotelResponseModel>> DeleteHotelAsync(Guid id)
        {
            var command = new DeleteHotelCommand
            {
                Id = id
            };

            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
