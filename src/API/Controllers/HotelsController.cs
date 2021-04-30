using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models.RequestModels;
using HotelReservation.Business.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;

namespace HotelReservation.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelsService _service;

        public HotelsController(IHotelsService service)
        {
            _service = service;
        }

        // GET: api/<HotelsController>
        [Authorize(Policy = "GetHotelsPermission")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelResponseModel>>> GetHotels()
        {
            var hotelsResponse = await _service.GetHotelsAsync();
            if (hotelsResponse == null) return NotFound("There is no hotels in system");

            return Ok(hotelsResponse);
        }

        // GET api/<HotelsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelResponseModel>> GetHotel(int id)
        {
            var hotelResponse = await _service.GetAsync(id);
            if (hotelResponse == null) return NotFound();

            return Ok(hotelResponse);
        }

        // POST api/<HotelsController>
        [Authorize(Policy = "PostHotelsPermission")]
        [HttpPost]
        public async Task<ActionResult<HotelResponseModel>> CreateHotel([FromBody] HotelRequestModel hotelRequest)
        {
            var hotelResponse = await _service.CreateAsync(hotelRequest);

            return Ok(hotelResponse);
        }

        // PUT api/<HotelsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<HotelsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
