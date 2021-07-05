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
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RoomResponseModel>> GetRoomByIdAsync(Guid id)
        {
            var roomModel = await _roomsService.GetAsync(id);
            var roomResponseModel = _mapper.Map<RoomResponseModel>(roomModel);

            return Ok(roomResponseModel);
        }

        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpPost]
        public async Task<ActionResult<RoomResponseModel>> CreateRoomAsync([FromBody] RoomRequestModel roomRequestModel)
        {
            var roomModel = _mapper.Map<RoomModel>(roomRequestModel);
            var createdRoom = await _roomsService.CreateAsync(roomModel);
            var createdRoomResponseModel = _mapper.Map<RoomResponseModel>(createdRoom);

            return Ok(createdRoomResponseModel);
        }

        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<RoomResponseModel>> UpdateRoomAsync(Guid id, [FromBody] RoomRequestModel roomRequestModel)
        {
            var roomModel = _mapper.Map<RoomModel>(roomRequestModel);
            var createdRoom = await _roomsService.UpdateAsync(id, roomModel);
            var createdRoomResponseModel = _mapper.Map<RoomResponseModel>(createdRoom);

            return Ok(createdRoomResponseModel);
        }

        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<RoomResponseModel>> DeleteRoomAsync(Guid id)
        {
            var deletedRoomModel = await _roomsService.DeleteAsync(id);
            var deletedRoomResponseModel = _mapper.Map<RoomResponseModel>(deletedRoomModel);

            return Ok(deletedRoomResponseModel);
        }
    }
}
