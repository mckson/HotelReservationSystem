using HotelReservation.API.Application.Commands.Image;
using HotelReservation.API.Application.Queries.Image;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Constants;
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
    public class ImagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ImagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Method that returns image of the hotel as FileContentResult
        /// </summary>
        /// <param name="id">Requested image id</param>
        /// <returns>Image of the hotel as FileContentResult</returns>
        /// <response code="404">When image of hotel with such id does not exist</response>
        [AllowAnonymous]
        [HttpGet("Hotel/{id:guid}")]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetHotelImageAsync(Guid id)
        {
            var query = new GetHotelImageByIdQuery
            {
                Id = id
            };

            var response = await _mediator.Send(query);
            return response;
        }

        /// <summary>
        /// Method that returns image of the room as FileContentResult
        /// </summary>
        /// <param name="id">Requested image id</param>
        /// <returns>Image of the room as FileContentResult</returns>
        /// <response code="404">When image of hotel with such id does not exist</response>
        [AllowAnonymous]
        [HttpGet("Room/{id:guid}")]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetRoomImageAsync(Guid id)
        {
            var query = new GetRoomImageByIdQuery
            {
                Id = id
            };

            var response = await _mediator.Send(query);
            return response;
        }

        /// <summary>
        /// Method that allow to create image for a hotel
        /// </summary>
        /// <remarks>Creation of hotel's images is allowed for authorized admin or manager only</remarks>
        /// <param name="command">Image details such as (base64 string with image content, name, id of the hotel)</param>
        /// <returns>No content</returns>
        /// <response code="204">When image successfully added</response>
        /// <response code="403">When manager has no permissions to manage current hotel</response>
        /// <response code="404">When hotel with specified id does not exist</response>
        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpPost("Hotel")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AddHotelImageAsync([FromBody] CreateHotelImageCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Method that allow to create image for a room
        /// </summary>
        /// <remarks>Creation of room's images is allowed for authorized admin or manager only</remarks>
        /// <param name="command">Image details such as (base64 string with image content, name, id of the room)</param>
        /// <returns>No content</returns>
        /// <response code="204">When image successfully added</response>
        /// <response code="404">When room with specified id does not exist</response>
        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpPost("Room")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HotelImageResponseModel>> AddRoomImageAsync([FromBody] CreateRoomImageCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Method that allow to delete image for a hotel
        /// </summary>
        /// <remarks>Deletion of hotel's images is allowed for authorized admin or manager only</remarks>
        /// <param name="id">Image's identifier</param>
        /// <returns>No content</returns>
        /// <response code="204">When image successfully deleted</response>
        /// <response code="403">When manager has no permissions to manage current hotel</response>
        /// <response code="404">When hotel's image with specified id does not exist</response>
        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpDelete("Hotel/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HotelImageResponseModel>> DeleteHotelImageAsync(Guid id)
        {
            var command = new DeleteHotelImageCommand
            {
                Id = id
            };

            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Method that allow to delete image for a room
        /// </summary>
        /// <remarks>Deletion of room's images is allowed for authorized admin or manager only</remarks>
        /// <param name="id">Image's identifier</param>
        /// <returns>No content</returns>
        /// <response code="204">When image successfully deleted</response>
        /// <response code="404">When room's image with specified id does not exist</response>
        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpDelete("Room/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HotelImageResponseModel>> DeleteRoomImageAsync(Guid id)
        {
            var command = new DeleteRoomImageCommand
            {
                Id = id
            };

            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Method that allow to change main picture to picture with specified id
        /// </summary>
        /// <param name="id">Identifier of the picture that need to be hotel's main picture</param>
        /// <returns>No content</returns>
        /// <response code="204">When image successfully updated</response>
        /// <response code="403">When manager has no permissions to manage current hotel</response>
        /// <response code="404">When hotel's image with specified id does not exist, or when hotel with specified id does not exist</response>
        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpPut("Hotel/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HotelImageResponseModel>> UpdateImageToMainAsync(Guid id)
        {
            var command = new UpdateHotelImageToMainCommand
            {
                Id = id
            };

            await _mediator.Send(command);
            return NoContent();
        }
    }
}
