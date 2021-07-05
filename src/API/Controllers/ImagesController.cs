using AutoMapper;
using HotelReservation.API.Models.RequestModels;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Constants;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
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
        private const string DefaultType = "image/jpeg";
        private readonly IHotelImagesService _hotelImagesService;
        private readonly IMapper _mapper;
        private readonly IRoomImagesService _roomImagesService;

        public ImagesController(
            IHotelImagesService hotelImagesService,
            IRoomImagesService roomImagesService,
            IMapper mapper)
        {
            _hotelImagesService = hotelImagesService;
            _roomImagesService = roomImagesService;
            _mapper = mapper;
        }

        [HttpGet("Hotel/{id:guid}")]
        public async Task<ActionResult> GetHotelImageAsync(Guid id)
        {
            var imageModel = await _hotelImagesService.GetAsync(id);

            imageModel.Type ??= DefaultType;
            var image = new FileContentResult(imageModel.Image, imageModel.Type) { FileDownloadName = imageModel.Name };
            return image;
        }

        [HttpGet("Room/{id:guid}")]
        public async Task<ActionResult> GetRoomImageAsync(Guid id)
        {
            var imageModel = await _roomImagesService.GetAsync(id);

            imageModel.Type ??= DefaultType;
            var image = new FileContentResult(imageModel.Image, imageModel.Type) { FileDownloadName = imageModel.Name };
            return image;
        }

        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpPost("Hotel")]
        public async Task<ActionResult<HotelImageResponseModel>> AddHotelImageAsync([FromBody] HotelImageRequestModel hotelImageRequest)
        {
            var imageModel = _mapper.Map<HotelImageModel>(hotelImageRequest);
            await _hotelImagesService.CreateAsync(imageModel);

            return Ok();
        }

        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpPost("Room")]
        public async Task<ActionResult<HotelImageResponseModel>> AddRoomImageAsync([FromBody] RoomImageRequestModel roomImageRequest)
        {
            var imageModel = _mapper.Map<RoomImageModel>(roomImageRequest);
            await _roomImagesService.CreateAsync(imageModel);

            return Ok();
        }

        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpDelete("Hotel/{id:guid}")]
        public async Task<ActionResult<HotelImageResponseModel>> DeleteHotelImageAsync(Guid id)
        {
            await _hotelImagesService.DeleteAsync(id);

            return Ok();
        }

        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpDelete("Room/{id:guid}")]
        public async Task<ActionResult<HotelImageResponseModel>> DeleteRoomImageAsync(Guid id)
        {
            await _roomImagesService.DeleteAsync(id);

            return Ok();
        }

        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpPut("Hotel/{id:guid}")]
        public async Task<ActionResult<HotelImageResponseModel>> UpdateImageToMainAsync(Guid id)
        {
            await _hotelImagesService.ChangeImageToMainAsync(id);

            return Ok();
        }
    }
}
