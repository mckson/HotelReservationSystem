using AutoMapper;
using HotelReservation.API.Models.RequestModels;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Constants;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelsService _service;
        private readonly IMapper _mapper;

        public HotelsController(IHotelsService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<HotelResponseModel> GetHotels()
        {
            var hotelsResponse = _mapper.Map<IEnumerable<HotelResponseModel>>(_service.GetHotels());

            return hotelsResponse;
        }

        // GET api/<HotelsController>/5
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<HotelResponseModel>> GetHotelByIdAsync(int id)
        {
            var hotelModel = await _service.GetAsync(id);
            var hotelResponse = _mapper.Map<HotelResponseModel>(hotelModel);

            if (hotelResponse == null)
                return NotFound();

            return Ok(hotelResponse);
        }

        // GET api/<HotelsController>/hotelName
        [AllowAnonymous]
        [HttpGet("{name}")]
        public async Task<ActionResult<HotelResponseModel>> GetHotelByIdAsync(string name)
        {
            var hotelResponse = _mapper.Map<HotelResponseModel>(await _service.GetHotelByNameAsync(name));

            return Ok(hotelResponse);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = Policies.AdminManagerPermission)]
        [HttpPost]
        public async Task<ActionResult<HotelResponseModel>> CreateHotelAsync([FromBody] HotelRequestModel hotelRequest)
        {
            var userClaims = User.Claims;
            var hotelResponse =
                _mapper.Map<HotelResponseModel>(await _service.CreateAsync(
                    _mapper.Map<HotelModel>(hotelRequest),
                    userClaims));

            return Ok(hotelResponse);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = Policies.AdminManagerPermission)]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<HotelResponseModel>> UpdateHotelAsync(int id, [FromBody] HotelRequestModel hotelRequest)
        {
            var userClaims = User.Claims;
            var hotelResponse =
                _mapper.Map<HotelResponseModel>(await _service.UpdateAsync(
                    id,
                    _mapper.Map<HotelModel>(hotelRequest),
                    userClaims));

            return Ok(hotelResponse);
        }

        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = "Bearer", Policy = Policies.AdminManagerPermission)]
        public async Task<ActionResult> DeleteHotelAsync(int id)
        {
            var userClaims = User.Claims;
            await _service.DeleteAsync(id, userClaims);
            return Ok();
        }
    }
}
