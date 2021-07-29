using HotelReservation.API.Application.Commands.User;
using HotelReservation.API.Application.Queries.User;
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
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<UserBriefResponseModel>>> GetAllUsersAsync()
        {
            var query = new GetAllUsersQuery();
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        /// <summary>
        /// Method that returns paged and filtered UserResponseModels
        /// </summary>
        /// <remarks>Method is allowed for authenticated admin only</remarks>
        /// <param name="paginationFilter">Pagination filter (pageNumber, pageSize)</param>
        /// <param name="usersFilter">User filter (email)</param>
        /// <returns>BasePagedResponseModel with page of filtered UserResponseModels</returns>
        /// <response code="200">Returns page of UserResponseModels</response>
        /// <response code="401">When user is unauthenticated</response>
        /// <response code="403">When user has no permissions to get paged users</response>
        /// <response code="404">When no users, that satisfy filter parameters, were found</response>
        [Authorize(Policy = Policies.AdminPermission)]
        [HttpGet]
        [ProducesResponseType(typeof(BasePagedResponseModel<UserResponseModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BasePagedResponseModel<UserResponseModel>>> GetPagedAndFilteredUsersAsync(
            [FromQuery] PaginationFilter paginationFilter, [FromQuery] UsersFilter usersFilter)
        {
            var query = new GetPagedFilteredUsersQuery
            {
                PaginationFilter = paginationFilter,
                UsersFilter = usersFilter,
            };

            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [Authorize(Policy = Policies.AdminPermission)]
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<UserPromptResponseModel>>> GetUserSearchVariantsAsync(
            [FromQuery] UsersFilter usersFilter)
        {
            var query = new GetUserSearchVariantsQuery
            {
                UsersFilter = usersFilter
            };

            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserResponseModel>> GetUserByIdAsync(Guid id)
        {
            var query = new GetUserByIdQuery
            {
                Id = id
            };

            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [Authorize(Policy = Policies.AdminPermission)]
        [HttpPost]
        public async Task<ActionResult<UserResponseModel>> CreateUserAsync([FromBody] CreateUserCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [Authorize]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<UserResponseModel>> UpdateUserAsync(Guid id, [FromBody] UpdateUserCommand command)
        {
            // var currentUserClaims = User.Claims;
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
        public async Task<ActionResult<UserResponseModel>> DeleteUserByIdAsync(Guid id)
        {
            var command = new DeleteUserCommand
            {
                Id = id
            };

            await _mediator.Send(command);
            return Ok();
        }
    }
}
