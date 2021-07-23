using HotelReservation.API.Application.Commands.Room;
using HotelReservation.API.Application.Queries.Room;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Business.Constants;
using HotelReservation.Data.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [HttpGet("Names")]
        public async Task<IActionResult> GetAllRoomUniqueNames()
        {
            var query = new GetAllRoomUniqueNamesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("Names/{hotelId:guid}")]
        public async Task<IActionResult> GetAllRoomUniqueNames(Guid hotelId)
        {
            var query = new GetHotelRoomsUniqueNamesQuery
            {
                HotelId = hotelId
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("Numbers/{hotelId:guid}")]
        public async Task<IActionResult> GetAllRoomUniqueNumbers(Guid hotelId)
        {
            var query = new GetHotelRoomsUniqueNumbersQuery()
            {
                HotelId = hotelId
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<BasePagedResponseModel<RoomResponseModel>>> GetRoomsAsync(
            [FromQuery] PaginationFilter paginationFilter, [FromQuery] RoomsFilter roomsFilter)
        {
            var query = new GetPagedFilteredRoomsQuery
                {
                    PaginationFilter = paginationFilter,
                    RoomsFilter = roomsFilter,
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

        /// <summary>
        /// Method that allow lock room for reservation for time that is specified in app settings.
        /// To block room needed room id and user id reserving room. When unauthorized user started reservation process room could be locked for shorter time
        /// </summary>
        /// <param name="id">Room identifier</param>
        /// <returns>No content</returns>
        /// <remarks>POST Rooms/e7dd3b68-4999-4e03-9980-14e238da05d4/lock</remarks>
        /// <response code="204">When room is successfully locked</response>
        /// <response code="403">When room is already locked by someone</response>
        /// <response code="404">When room cannot be found</response>
        [AllowAnonymous]
        [HttpPost("{id:guid}/lock")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoomResponseModel>> LockRoomAsync(Guid id)
        {
            var command = new LockRoomCommand
            {
                Id = id
            };
            await _mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Method that allow to unlock room for other users when current user finished his reservation or decided cancel it.
        /// /// </summary>
        /// <param name="id">Room identifier</param>
        /// <returns>No content</returns>
        /// <remarks>POST Rooms/e7dd3b68-4999-4e03-9980-14e238da05d4/unlock</remarks>
        /// <response code="204">When room is successfully unlocked</response>
        /// <response code="403">When room is already locked by someone</response>
        /// <response code="404">When room cannot be found or room is already unlocked</response>
        [AllowAnonymous]
        [HttpPost("{id:guid}/unlock")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoomResponseModel>> UnlockRoomAsync(Guid id)
        {
            var command = new UnlockRoomCommand
            {
                Id = id
            };
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
