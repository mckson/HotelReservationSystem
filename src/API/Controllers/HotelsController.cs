using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservation.Business;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models.RequestModels;
using HotelReservation.Business.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<IEnumerable<HotelResponseModel>> GetHotels()
        {
            var hotelsResponse = _service.GetHotels();
            if (hotelsResponse == null)
                return NotFound("There is no hotels in system");

            return Ok(hotelsResponse);
        }

        // GET api/<HotelsController>/5
        [Authorize(Policy = "GetHotelsPermission")]
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelResponseModel>> GetHotel(int id)
        {
            var hotelResponse = await _service.GetAsync(id);
            if (hotelResponse == null)
                return NotFound();

            return Ok(hotelResponse);
        }

        // POST api/<HotelsController>
        [Authorize(Policy = "PostHotelsPermission")]
        [HttpPost]
        public async Task<ActionResult<HotelResponseModel>> CreateHotel([FromBody] HotelRequestModel hotelRequest)
        {
            try
            {
                var hotelResponse = await _service.CreateAsync(hotelRequest);

                return Ok(hotelResponse);
            }
            catch (DataException ex)
            {
                if (ex.Status == ErrorStatus.NotFound)
                    return NotFound($"{ex.Status}: {ex.Message}");

                if (ex.Status == ErrorStatus.HasLinkedEntity)
                    return BadRequest($"{ex.Status}: {ex.Message}");

                return BadRequest();
            }
        }

        // PUT api/<HotelsController>/5
        [Authorize(Policy = "UpdateHotelsPermission")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<HotelResponseModel>> PutAsync(int id, [FromBody] HotelRequestModel hotelRequest)
        {
            try
            {
                var hotelResponse = await _service.UpdateAsync(id, hotelRequest);

                return Ok(hotelResponse);
            }
            catch (DataException ex)
            {
                if (ex.Status == ErrorStatus.NotFound)
                    return NotFound($"{ex.Status}: {ex.Message}");

                if (ex.Status == ErrorStatus.HasLinkedEntity)
                    return BadRequest($"{ex.Status}: {ex.Message}");

                return BadRequest();
            }
        }

        // DELETE api/<HotelsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok();
            }
            catch (DataException ex)
            {
                if (ex.Status == ErrorStatus.NotFound)
                    return NotFound($"{ex.Status}: {ex.Message}");

                if (ex.Status == ErrorStatus.HasLinkedEntity)
                    return BadRequest($"{ex.Status}: {ex.Message}");

                return BadRequest();
            }
        }
    }
}
