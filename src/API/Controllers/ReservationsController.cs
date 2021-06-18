using AutoMapper;
using HotelReservation.API.Models.RequestModels;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Constants;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public ReservationsController(
            IReservationsService reservationsService,
            IMapper mapper)
        {
            _reservationsService = reservationsService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ReservationResponseModel>> GetReservationByIdAsync(int id)
        {
            var userClaims = User.Claims;

            var reservationModel = await _reservationsService.GetAsync(id, userClaims);
            var reservationResponseModel = _mapper.Map<ReservationResponseModel>(reservationModel);

            return Ok(reservationResponseModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<ReservationResponseModel>> CreateReservationAsync([FromBody] ReservationRequestModel reservationRequestModel)
        {
            var reservationModel = _mapper.Map<ReservationModel>(reservationRequestModel);

            reservationModel.ReservationServices = reservationRequestModel.Services.Select(service => new ReservationServiceModel { ServiceId = service }).ToList();

            reservationModel.ReservationRooms = reservationRequestModel.Rooms.Select(room => new ReservationRoomModel { RoomId = room }).ToList();

            var createdReservationModel = await _reservationsService.CreateAsync(reservationModel);
            var createdReservationResponseModel = _mapper.Map<ReservationResponseModel>(createdReservationModel);

            return Ok(createdReservationResponseModel);
        }

        [Authorize(Policy = Policies.AdminPermission)]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ReservationResponseModel>> DeleteReservation(int id)
        {
            var userClaims = User.Claims;

            var deletedReservationModel = await _reservationsService.DeleteAsync(id, userClaims);
            var deletedReservationResponseModel = _mapper.Map<ReservationResponseModel>(deletedReservationModel);

            return Ok(deletedReservationResponseModel);
        }
    }
}
