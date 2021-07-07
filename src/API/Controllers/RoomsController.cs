using HotelReservation.API.Commands.Room;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.API.Queries.Room;
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
    public class RoomsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoomsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet("All")]
        public async Task<IActionResult> GetAllRooms()
        {
            var query = new GetAllRoomsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<BasePagedResponseModel<RoomResponseModel>>> GetRoomsAsync(
            [FromQuery] PaginationFilter paginationFilter, [FromQuery] RoomsFilter roomsFilter)
        {
            var route = Request.Path.Value;

            var query = new GetPagedFilteredRoomsQuery
                {
                    PaginationFilter = paginationFilter,
                    RoomsFilter = roomsFilter,
                    Route = route
                };

            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RoomResponseModel>> GetRoomByIdAsync(Guid id)
        {
            var query = new GetRoomByIdQuery(id);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpPost]
        public async Task<ActionResult<RoomResponseModel>> CreateRoomAsync([FromBody] CreateRoomCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<RoomResponseModel>> UpdateRoomAsync(Guid id, [FromBody] UpdateRoomCommand command)
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
        public async Task<ActionResult<RoomResponseModel>> DeleteRoomAsync(Guid id)
        {
            var command = new DeleteRoomCommand { Id = id };
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
