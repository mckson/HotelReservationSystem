using HotelReservation.API.Commands.User;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.API.Queries.User;
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
    [Authorize(Policy = Policies.AdminPermission)]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseModel>>> GetAllUsersAsync()
        {
            var query = new GetAllUsersQuery();
            var response = await _mediator.Send(query);
            return Ok(response);
        }

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

        [HttpPost]
        public async Task<ActionResult<UserResponseModel>> CreateUserAsync([FromBody] CreateUserCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

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
