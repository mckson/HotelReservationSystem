using HotelReservation.API.Application.Commands.Hotel;
using HotelReservation.API.Application.Queries.Hotel;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Business.Constants;
using HotelReservation.Data.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Method to retrieve BasePagedResponseModel that contain page (means limited amount of elements with defined offset) of filtered hotels
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Hotels
        ///     {
        ///         "pageNumber" : 1,
        ///         "pageSize" : 15,
        ///         "managerId" : "6F9619FF-8B86-D011-B42D-00CF4FC964FF",
        ///         "name" : "HotelName",
        ///         "city" : "CityName"
        ///     }
        ///
        /// </remarks>
        /// <param name="paginationFilter">Pagination parameters (pageNumber, pageSize)</param>
        /// <param name="hotelsFilter">Filter parameters for hotels (managerId, name, city etc.)</param>
        /// <returns>BasePagedResponseModel that contain filtered collection of HotelResponseModel</returns>
        /// <response code="200">Returns BasePagedResponseModel with collection of HotelResponseModels</response>
        /// <response code="404">When no hotels, that are satisfy filter options, were created</response>
        [AllowAnonymous]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(BasePagedResponseModel<HotelResponseModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
        public async Task<BasePagedResponseModel<HotelResponseModel>> GetHotelsAsync([FromQuery] PaginationFilter paginationFilter, [FromQuery] HotelsFilter hotelsFilter)
        {
            var query = new GetPagedFilteredHotelsQuery
            {
                PaginationFilter = paginationFilter,
                HotelsFilter = hotelsFilter,
            };

            var response = await _mediator.Send(query);

            return response;
        }

        /// <summary>
        /// Method to retrieve a HotelResponseModel with provided identifier' value
        /// </summary>
        /// <param name="id">Value of hotel's identifier</param>
        /// <returns>HotelResponseModel with provided id value</returns>
        /// <response code="200">Returns needed HotelResponseModel</response>
        /// <response code="404">When hotel with provided id does not exist</response>
        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(HotelResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HotelResponseModel>> GetHotelByIdAsync(Guid id)
        {
            var query = new GetHotelByIdQuery { Id = id };
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        /// <summary>
        /// Method to create a hotel
        /// </summary>
        /// <remarks>Creation of hotel is allowed for authorized admin only</remarks>
        /// <param name="command">Properties' description of the hotel that need to be created</param>
        /// <returns>HotelResponseModel of created hotel</returns>
        /// <response code="200">Returns a HotelResponseModel with data about created hotel</response>
        /// <response code="401">When user is unauthenticated</response>
        /// <response code="403">When user is authenticated, but has no  permissions to create hotels (when user is not admin). Or, when some validation errors occurred</response>
        /// <response code="409">When there is an attempt to create hotel with location, that already was occupied </response>
        [Authorize(Policy = Policies.AdminPermission)]
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(HotelResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<HotelResponseModel>> CreateHotelAsync([FromBody] CreateHotelCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        /// <summary>
        /// Method to update info about existing hotel
        /// </summary>
        /// <remarks>Renovation of hotel is allowed for authorized admin only</remarks>
        /// <param name="id">Value of required hotel's identifier</param>
        /// <param name="command">Description of updated properties for the hotel</param>
        /// <returns>HotelResponseModel with info about updated hotel</returns>
        /// <response code="200">Returns a HotelResponseModel with data about updated hotel</response>
        /// <response code="401">When user is unauthenticated</response>
        /// <response code="403">When user is authenticated, but has no  permissions to update hotels (when user is not admin). Or, when some validation errors occurred</response>
        /// <response code="404">When hotel with provided id does not exist</response>
        /// <response code="409">When there is an attempt to update hotel with location, that already was occupied </response>
        /// <response code="422">When identifier of hotel for update is not matching with Id from route </response>
        [Authorize(Policy = Policies.AdminPermission)]
        [HttpPut("{id:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(HotelResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status422UnprocessableEntity)]
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

        /// <summary>
        /// Method to delete existing hotel
        /// </summary>
        /// <remarks>Deletion of hotel is allowed for authorized admin only</remarks>
        /// <param name="id">Value of required hotel's identifier</param>
        /// <returns>HotelResponseModel with info about deleted hotel</returns>
        /// <response code="200">Returns a HotelResponseModel with data about deleted hotel</response>
        /// <response code="401">When user is unauthenticated</response>
        /// <response code="403">When user is authenticated, but has no  permissions to update hotels (when user is not admin). Or, when some validation errors occurred</response>
        /// <response code="404">When hotel with provided id does not exist</response>
        [Authorize(Policy = Policies.AdminPermission)]
        [HttpDelete("{id:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(HotelResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HotelResponseModel>> DeleteHotelAsync(Guid id)
        {
            var command = new DeleteHotelCommand
            {
                Id = id
            };

            var response = await _mediator.Send(command);
            return Ok(response);
        }

        /// <summary>
        /// Method that allow to retrieve all hotel names and identifiers (for example, for search or select fields)
        /// </summary>
        /// <returns>Collection of HotelBriefResponse models</returns>
        /// <response code="200">Returns collection of HotelBriefResponse models</response>
        /// <response code="404">When no hotels were created yet</response>
        [AllowAnonymous]
        [HttpGet("All")]
        [ProducesResponseType(typeof(IEnumerable<HotelBriefResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<HotelBriefResponse>>> GetAllHotelsNameAndIdAsync()
        {
            var query = new GetAllHotelsNameAndIdQuery();
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("Search")]
        [ProducesResponseType(typeof(IEnumerable<HotelBriefResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<HotelFilterResponseModel>>> GetSearchHotelsAsync([FromQuery] HotelsFilter hotelsFilter)
        {
            var query = new GetHotelSearchVariantsQuery
            {
                HotelsFilter = hotelsFilter
            };

            var response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}
