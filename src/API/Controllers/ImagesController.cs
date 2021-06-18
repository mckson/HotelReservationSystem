using AutoMapper;
using HotelReservation.API.Models.RequestModels;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Constants;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;

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

        [HttpGet("{id}")]
        public async Task<ActionResult> GetImageAsync(int id)
        {
            var imageModel = await _imageService.GetAsync(id);

            // var imageResponse = _mapper.Map<ImageResponseModel>(imageModel);
            imageModel.Type ??= "image/jpeg";
            var image = new FileContentResult(imageModel.Image, imageModel.Type) { FileDownloadName = imageModel.Name };
            return image;
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = Policies.AdminManagerPermission)]
        [HttpPost]
        public async Task<ActionResult<ImageResponseModel>> AddImageAsync([FromBody] ImageRequestModel imageRequest)
        {
            var userClaims = User.Claims;

            var imageModel = _mapper.Map<ImageModel>(imageRequest);
            var addedImageModel = await _imageService.CreateAsync(imageModel, userClaims);

            // var addedImageResponse = _mapper.Map<ImageResponseModel>(addedImageModel);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = Policies.AdminManagerPermission)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ImageResponseModel>> DeleteImageAsync(int id)
        {
            var userClaims = User.Claims;
            var deletedImageModel = await _imageService.DeleteAsync(id, userClaims);

            // var deletedImageResponse = _mapper.Map<ImageResponseModel>(deletedImageModel);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = Policies.AdminManagerPermission)]
        [HttpPut("{id}")]
        public async Task<ActionResult<ImageResponseModel>> UpdateImageToMainAsync(int id)
        {
            var userClaims = User.Claims;
            var updatedImageModel = await _imageService.ChangeImageToMainAsync(id, userClaims);

            return Ok();
        }
    }
}
