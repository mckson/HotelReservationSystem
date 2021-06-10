using AutoMapper;
using HotelReservation.API.Models.RequestModels;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Constants;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<ImageResponseModel>> GetImageAsync(int id)
        {
            var imageModel = await _imageService.GetAsync(id);
            var imageResponse = _mapper.Map<ImageResponseModel>(imageModel);

            return Ok(imageResponse);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = Policies.AdminManagerPermission)]
        [HttpPost]
        public async Task<ActionResult<ImageResponseModel>> AddImageAsync([FromBody] ImageRequestModel imageRequest)
        {
            var imageModel = _mapper.Map<ImageModel>(imageRequest);
            var addedImageModel = await _imageService.CreateAsync(imageModel);
            var addedImageResponse = _mapper.Map<ImageResponseModel>(addedImageModel);

            return Ok(addedImageResponse);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = Policies.AdminManagerPermission)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ImageResponseModel>> DeleteImageAsync(int id)
        {
            var deletedImageModel = await _imageService.DeleteAsync(id);
            var deletedImageResponse = _mapper.Map<ImageResponseModel>(deletedImageModel);

            return Ok(deletedImageResponse);
        }
    }
}
