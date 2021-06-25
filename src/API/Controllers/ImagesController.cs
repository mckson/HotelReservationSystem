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
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public ImagesController(
            IImageService imageService,
            IMapper mapper)
        {
            _imageService = imageService;
            _mapper = mapper;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> GetImageAsync(Guid id)
        {
            var imageModel = await _imageService.GetAsync(id);

            imageModel.Type ??= "image/jpeg";
            var image = new FileContentResult(imageModel.Image, imageModel.Type) { FileDownloadName = imageModel.Name };
            return image;
        }

        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpPost]
        public async Task<ActionResult<ImageResponseModel>> AddImageAsync([FromBody] ImageRequestModel imageRequest)
        {
            var imageModel = _mapper.Map<ImageModel>(imageRequest);
            await _imageService.CreateAsync(imageModel);

            return Ok();
        }

        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ImageResponseModel>> DeleteImageAsync(Guid id)
        {
            await _imageService.DeleteAsync(id);

            return Ok();
        }

        [Authorize(Policy = Policies.AdminManagerPermission)]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ImageResponseModel>> UpdateImageToMainAsync(Guid id)
        {
            await _imageService.ChangeImageToMainAsync(id);

            return Ok();
        }
    }
}
