using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservation.API.Models.RequestModels;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Business.Interfaces;
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

        // GET api/<HotelsController>/5/Rooms
        [Authorize(Policy = "GetHotelsPermission")]
        [HttpGet("{id:int}/Rooms")]
        public async Task<ActionResult<IEnumerable<RoomResponseModel>>> GetHotelRooms([FromRoute] int id)
        {
            try
            {
                var roomsResponse = await _service.GetHotelRooms(id);

                return Ok(roomsResponse);
            }
            catch (DataException ex)
            {
                return ex.Status switch
                {
                    ErrorStatus.NotFound => NotFound($"{ex.Status}: {ex.Message}"),
                    ErrorStatus.HasLinkedEntity => BadRequest($"{ex.Status}: {ex.Message}"),
                    _ => BadRequest()
                };
            }
        }

        // GET api/<HotelsController>/5/Location
        [Authorize(Policy = "GetHotelsPermission")]
        [HttpGet("{id:int}/Location")]
        public async Task<ActionResult<LocationResponseModel>> GetHotelLocation([FromRoute] int id)
        {
            try
            {
                var locationResponse = await _service.GetHotelLocation(id);

                return Ok(locationResponse);
            }
            catch (DataException ex)
            {
                return ex.Status switch
                {
                    ErrorStatus.NotFound => NotFound($"{ex.Status}: {ex.Message}"),
                    ErrorStatus.HasLinkedEntity => BadRequest($"{ex.Status}: {ex.Message}"),
                    _ => BadRequest()
                };
            }
        }

        // GET api/<HotelsController>/5/Company
        [Authorize(Policy = "GetHotelsPermission")]
        [HttpGet("{id:int}/Company")]
        public async Task<ActionResult<CompanyResponseModel>> GetHotelCompany([FromRoute] int id)
        {
            try
            {
                var companyResponse = await _service.GetHotelCompany(id);

                return Ok(companyResponse);
            }
            catch (DataException ex)
            {
                return ex.Status switch
                {
                    ErrorStatus.NotFound => NotFound($"{ex.Status}: {ex.Message}"),
                    ErrorStatus.HasLinkedEntity => BadRequest($"{ex.Status}: {ex.Message}"),
                    _ => BadRequest()
                };
            }
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
                return ex.Status switch
                {
                    ErrorStatus.NotFound => NotFound($"{ex.Status}: {ex.Message}"),
                    ErrorStatus.HasLinkedEntity => BadRequest($"{ex.Status}: {ex.Message}"),
                    _ => BadRequest()
                };
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
                return ex.Status switch
                {
                    ErrorStatus.NotFound => NotFound($"{ex.Status}: {ex.Message}"),
                    ErrorStatus.HasLinkedEntity => BadRequest($"{ex.Status}: {ex.Message}"),
                    _ => BadRequest()
                };
            }
        }

        // DELETE api/<HotelsController>/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok();
            }
            catch (DataException ex)
            {
                return ex.Status switch
                {
                    ErrorStatus.NotFound => NotFound($"{ex.Status}: {ex.Message}"),
                    ErrorStatus.HasLinkedEntity => BadRequest($"{ex.Status}: {ex.Message}"),
                    _ => BadRequest()
                };
            }
        }
    }
}
