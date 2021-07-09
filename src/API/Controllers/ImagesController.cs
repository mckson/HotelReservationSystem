using HotelReservation.API.Application.Commands.Image;
using HotelReservation.API.Application.Queries.Image;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("Hotel/{id:guid}")]
        public async Task<ActionResult> GetHotelImageAsync(Guid id)
        {
            var query = new GetHotelImageByIdQuery
            {
                Id = id
            };

            var response = await _mediator.Send(query);
            return response;
        }

        [HttpGet("Room/{id:guid}")]
        public async Task<ActionResult> GetRoomImageAsync(Guid id)
        {
            var query = new GetRoomImageByIdQuery
            {
                Id = id
            };

            var response = await _mediator.Send(query);
            return response;
        }

        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpPost("Hotel")]
        public async Task<ActionResult> AddHotelImageAsync([FromBody] CreateHotelImageCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpPost("Room")]
        public async Task<ActionResult<HotelImageResponseModel>> AddRoomImageAsync([FromBody] CreateRoomImageCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpDelete("Hotel/{id:guid}")]
        public async Task<ActionResult<HotelImageResponseModel>> DeleteHotelImageAsync(Guid id)
        {
            var command = new DeleteHotelImageCommand
            {
                Id = id
            };

            await _mediator.Send(command);
            return Ok();
        }

        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpDelete("Room/{id:guid}")]
        public async Task<ActionResult<HotelImageResponseModel>> DeleteRoomImageAsync(Guid id)
        {
            var command = new DeleteRoomImageCommand
            {
                Id = id
            };

            await _mediator.Send(command);
            return Ok();
        }

        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpPut("Hotel/{id:guid}")]
        public async Task<ActionResult<HotelImageResponseModel>> UpdateImageToMainAsync(Guid id)
        {
            var command = new UpdateHotelImageToMainCommand
            {
                Id = id
            };

            await _mediator.Send(command);
            return Ok();
        }
    }
}
