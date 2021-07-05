using AutoMapper;
using HotelReservation.API.Models.RequestModels;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Constants;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomViewsController : ControllerBase
    {
        private readonly IRoomViewsService _roomViewsService;
        private readonly IMapper _mapper;

        public RoomViewsController(
            IRoomViewsService roomViewsService,
            IMapper mapper)
        {
            _roomViewsService = roomViewsService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<RoomViewResponseModel>> GetAllRoomViews()
        {
            var roomViewModels = _roomViewsService.GetAllRoomViews();
            var roomViewResponseModels = _mapper.Map<IEnumerable<RoomViewResponseModel>>(roomViewModels);

            return Ok(roomViewResponseModels);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RoomViewResponseModel>> GetRoomViewByIdAsync(Guid id)
        {
            var roomViewModel = await _roomViewsService.GetAsync(id);
            var roomViewResponseModel = _mapper.Map<RoomViewResponseModel>(roomViewModel);

            return Ok(roomViewResponseModel);
        }

        [Authorize(Policy = Policies.AdminPermission)]
        [HttpPost]
        public async Task<ActionResult<RoomViewResponseModel>> CreateRoomViewModelAsync(
            [FromBody] RoomViewRequestModel roomViewRequestModel)
        {
            var roomViewModel = _mapper.Map<RoomViewModel>(roomViewRequestModel);
            var createdRoomViewModel = await _roomViewsService.CreateAsync(roomViewModel);
            var createdRoomViewResponseModel = _mapper.Map<RoomViewResponseModel>(createdRoomViewModel);

            return Ok(createdRoomViewResponseModel);
        }

        [Authorize(Policy = Policies.AdminPermission)]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<RoomViewResponseModel>> UpdateRoomViewAsync(
            Guid id,
            [FromBody] RoomViewRequestModel roomViewRequestModel)
        {
            var roomViewModel = _mapper.Map<RoomViewModel>(roomViewRequestModel);
            var updatedRoomViewModel = await _roomViewsService.UpdateAsync(id, roomViewModel);
            var updatedRoomViewResponseModel = _mapper.Map<RoomViewResponseModel>(updatedRoomViewModel);

            return Ok(updatedRoomViewResponseModel);
        }

        [Authorize(Policy = Policies.AdminPermission)]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<RoomViewResponseModel>> DeleteRoomViewAsync(Guid id)
        {
            var deletedRoomViewModel = await _roomViewsService.DeleteAsync(id);
            var deletedRoomViewResponseModel = _mapper.Map<RoomViewResponseModel>(deletedRoomViewModel);

            return Ok(deletedRoomViewResponseModel);
        }
    }
}
