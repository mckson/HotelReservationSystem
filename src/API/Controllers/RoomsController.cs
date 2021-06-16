﻿using AutoMapper;
using HotelReservation.API.Helpers;
using HotelReservation.API.Models.RequestModels;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Constants;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Data.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRoomsService _roomsService;
        private readonly IUriService _uriService;

        public RoomsController(
            IRoomsService roomsService,
            IMapper mapper,
            IUriService uriService)
        {
            _roomsService = roomsService;
            _mapper = mapper;
            _uriService = uriService;
        }

        // [AllowAnonymous]
        // [HttpGet]
        // public ActionResult<IEnumerable<RoomResponseModel>> GetAllRooms()
        // {
        //     var roomModels = _roomsService.GetAllRooms();
        //     var roomResponseModels = _mapper.Map<IEnumerable<RoomResponseModel>>(roomModels);
        //     return Ok(roomResponseModels);
        // }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<BasePagedResponseModel<RoomResponseModel>>> GetRoomsAsync(
            [FromQuery] PaginationFilter paginationFilter, [FromQuery] RoomsFilter roomsFilter)
        {
            var route = Request.Path.Value;

            var totalRooms = await _roomsService.GetCountAsync(roomsFilter);

            paginationFilter.PageSize ??= totalRooms;

            var validFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize.Value);
            var roomModels = _roomsService.GetPagedRooms(validFilter, roomsFilter);

            var roomsResponse = _mapper.Map<IEnumerable<RoomResponseModel>>(roomModels);

            var pagedRoomsResponse =
                PaginationHelper.CreatePagedResponseModel(roomsResponse, validFilter, totalRooms, _uriService, route);

            return Ok(pagedRoomsResponse);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<RoomResponseModel>> GetRoomByIdAsync(int id)
        {
            var roomModel = await _roomsService.GetAsync(id);
            var roomResponseModel = _mapper.Map<RoomResponseModel>(roomModel);

            return Ok(roomResponseModel);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = Policies.AdminManagerPermission)]
        [HttpPost]
        public async Task<ActionResult<RoomResponseModel>> CreateRoomAsync([FromBody] RoomRequestModel roomRequestModel)
        {
            var userClaims = User.Claims;
            var roomModel = _mapper.Map<RoomModel>(roomRequestModel);
            var createdRoom = await _roomsService.CreateAsync(roomModel, userClaims);
            var createdRoomResponseModel = _mapper.Map<RoomResponseModel>(createdRoom);

            return Ok(createdRoomResponseModel);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = Policies.AdminManagerPermission)]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<RoomResponseModel>> UpdateRoomAsync(int id, [FromBody] RoomRequestModel roomRequestModel)
        {
            var userClaims = User.Claims;
            var roomModel = _mapper.Map<RoomModel>(roomRequestModel);
            var createdRoom = await _roomsService.UpdateAsync(id, roomModel, userClaims);
            var createdRoomResponseModel = _mapper.Map<RoomResponseModel>(createdRoom);

            return Ok(createdRoomResponseModel);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = Policies.AdminManagerPermission)]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<RoomResponseModel>> DeleteRoomAsync(int id)
        {
            var userClaims = User.Claims;
            var deletedRoomModel = await _roomsService.DeleteAsync(id, userClaims);
            var deletedRoomResponseModel = _mapper.Map<RoomResponseModel>(deletedRoomModel);

            return Ok(deletedRoomResponseModel);
        }
    }
}
