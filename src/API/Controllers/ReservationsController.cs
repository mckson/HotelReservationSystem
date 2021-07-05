using AutoMapper;
using HotelReservation.API.Helpers;
using HotelReservation.API.Models.RequestModels;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Constants;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Data.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationsService _reservationsService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public ReservationsController(
            IReservationsService reservationsService,
            IUriService uriService,
            IMapper mapper)
        {
            _reservationsService = reservationsService;
            _uriService = uriService;
            _mapper = mapper;
        }

        public async Task<ActionResult<BasePagedResponseModel<ReservationBriefResponseModel>>> GetFilteredReservationsAsync(
            [FromQuery] PaginationFilter paginationFilter,
            [FromQuery] ReservationsFilter reservationsFilter)
        {
            var route = Request.Path.Value;

            var totalReservations = await _reservationsService.GetReservationsCountAsync(reservationsFilter);

            paginationFilter.PageSize ??= totalReservations;

            var validFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize.Value);
            var reservationModels = _reservationsService.GetPagedReservations(validFilter, reservationsFilter);

            var reservationResponses = _mapper.Map<IEnumerable<ReservationBriefResponseModel>>(reservationModels);
            var pagedReservationResponses = PaginationHelper.CreatePagedResponseModel(
                reservationResponses,
                validFilter,
                totalReservations,
                _uriService,
                route);

            return Ok(pagedReservationResponses);
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ReservationResponseModel>> GetReservationByIdAsync(Guid id)
        {
            var reservationModel = await _reservationsService.GetAsync(id);
            var reservationResponseModel = _mapper.Map<ReservationResponseModel>(reservationModel);

            return Ok(reservationResponseModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<ReservationResponseModel>> CreateReservationAsync([FromBody] ReservationRequestModel reservationRequestModel)
        {
            var reservationModel = _mapper.Map<ReservationModel>(reservationRequestModel);

            reservationModel.ReservationServices = reservationRequestModel.Services.Select(service => new ReservationServiceModel { ServiceId = Guid.Parse(service) }).ToList();

            reservationModel.ReservationRooms = reservationRequestModel.Rooms.Select(room => new ReservationRoomModel { RoomId = Guid.Parse(room) }).ToList();

            var createdReservationModel = await _reservationsService.CreateAsync(reservationModel);
            var createdReservationResponseModel = _mapper.Map<ReservationResponseModel>(createdReservationModel);

            return Ok(createdReservationResponseModel);
        }

        [Authorize(Policy = Policies.AdminPermission)]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ReservationResponseModel>> DeleteReservation(Guid id)
        {
            var deletedReservationModel = await _reservationsService.DeleteAsync(id);
            var deletedReservationResponseModel = _mapper.Map<ReservationResponseModel>(deletedReservationModel);

            return Ok(deletedReservationResponseModel);
        }
    }
}
