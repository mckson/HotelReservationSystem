﻿using AutoMapper;
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

        [Authorize(AuthenticationSchemes = "Bearer", Policy = Policies.AdminPermission)]
        [HttpGet]
        public ActionResult<IEnumerable<ReservationResponseModel>> GetAllReservations()
        {
            var userClaims = User.Claims;

            var reservationModels = _reservationsService.GetAllReservations(userClaims);
            var reservationResponseModels = _mapper.Map<IEnumerable<ReservationResponseModel>>(reservationModels);

            return Ok(reservationResponseModels);
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
            var userClaims = User.Claims;

            var reservationModel = _mapper.Map<ReservationModel>(reservationRequestModel);

            reservationModel.ReservationServices = new List<ReservationServiceModel>();
            reservationModel.ReservationServices.AddRange(
                reservationRequestModel.Services.Select(service => new ReservationServiceModel { ServiceId = service }).ToList());

            reservationModel.ReservationRooms = new List<ReservationRoomModel>();
            reservationModel.ReservationRooms.AddRange(
                reservationRequestModel.Rooms.Select(room => new ReservationRoomModel { RoomId = room }).ToList());

            var createdReservationModel = await _reservationsService.CreateAsync(reservationModel, userClaims);
            var createdReservationResponseModel = _mapper.Map<ReservationResponseModel>(createdReservationModel);

            return Ok(createdReservationResponseModel);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = Policies.AdminPermission)]
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
