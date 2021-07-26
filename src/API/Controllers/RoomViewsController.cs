using HotelReservation.API.Application.Commands.RoomView;
using HotelReservation.API.Application.Queries.RoomView;
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
    public class RoomViewsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoomViewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomViewResponseModel>>> GetPagedFilteredRoomViewsAsync(
            [FromQuery] PaginationFilter paginationFilter,
            [FromQuery] RoomViewsFilter roomViewsFilter)
        {
            var query = new GetPagedFilteredRoomViewsQuery
            {
                PaginationFilter = paginationFilter,
                RoomViewsFilter = roomViewsFilter
            };

            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<RoomViewFilterResponseModel>>> GetSearchRoomViewsAsync(
            [FromQuery] RoomViewsFilter roomViewsFilter)
        {
            var query = new GetRoomViewSearchVariantsQuery
            {
                RoomViewsFilter = roomViewsFilter
            };

            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RoomViewResponseModel>> GetRoomViewByIdAsync(Guid id)
        {
            var query = new GetRoomViewByIdQuery
            {
                Id = id
            };
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [Authorize(Policy = Policies.AdminPermission)]
        [HttpPost]
        public async Task<ActionResult<RoomViewResponseModel>> CreateRoomViewModelAsync(
            [FromBody] CreateRoomViewCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [Authorize(Policy = Policies.AdminPermission)]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<RoomViewResponseModel>> UpdateRoomViewAsync(
            Guid id,
            [FromBody] UpdateRoomViewCommand command)
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

        [Authorize(Policy = Policies.AdminPermission)]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<RoomViewResponseModel>> DeleteRoomViewAsync(Guid id)
        {
            var command = new DeleteRoomViewCommand
            {
                Id = id
            };
            var response = await _mediator.Send(command);

            return Ok(response);
        }
    }
}
