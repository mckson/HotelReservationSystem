using HotelReservation.API.Application.Commands.Reservation;
using HotelReservation.API.Application.Queries.Reservation;
using HotelReservation.API.Models.ResponseModels;
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
    public class ReservationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReservationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ActionResult<BasePagedResponseModel<ReservationBriefResponseModel>>> GetFilteredReservationsAsync(
            [FromQuery] PaginationFilter paginationFilter,
            [FromQuery] ReservationsFilter reservationsFilter)
        {
            var route = Request.Path.Value;

            var query = new GetPagedFilteredReservationsQuery
            {
                PaginationFilter = paginationFilter,
                ReservationsFilter = reservationsFilter,
                Route = route
            };

            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ReservationResponseModel>> GetReservationByIdAsync(Guid id)
        {
            var query = new GetReservationByIdQuery
            {
                Id = id
            };

            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<ReservationBriefResponseModel>> CreateReservationAsync([FromBody] CreateReservationCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [Authorize(Policy = Policies.AdminPermission)]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ReservationBriefResponseModel>> DeleteReservation(Guid id)
        {
            var command = new DeleteReservationCommand
            {
                Id = id
            };

            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
