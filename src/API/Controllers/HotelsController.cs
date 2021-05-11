using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HotelReservation.API.Models.RequestModels;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
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
        private readonly IMapper _mapper;

        public HotelsController(IHotelsService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: api/<HotelsController>
        // [Authorize(Policy = "GetHotelsPermission")]
        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<HotelResponseModel> GetHotels()
        {
            var hotelsResponse = _mapper.Map<IEnumerable<HotelResponseModel>>(_service.GetHotels());
            /*if (hotelsResponse == null)
                return NotFound("There is no hotels in system");*/

            return hotelsResponse;
        }

        // GET api/<HotelsController>/5
        // [Authorize(Policy = "GetHotelsPermission")]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelResponseModel>> GetHotel(int id)
        {
            var hotelResponse = _mapper.Map<HotelResponseModel>(await _service.GetAsync(id));
            if (hotelResponse == null)
                return NotFound();

            return Ok(hotelResponse);
        }

        /*// GET api/<HotelsController>/5/Rooms
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
        }*/

        // POST api/<HotelsController>
        [Authorize(Policy = "PostHotelsPermission")]
        // [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<HotelResponseModel>> CreateHotel([FromBody] HotelRequestModel hotelRequest)
        {
            try
            {
                var hotelResponse =
                    _mapper.Map<HotelResponseModel>(await _service.CreateAsync(_mapper.Map<HotelModel>(hotelRequest)));

                return Ok(hotelResponse);
            }
            catch (BusinessException ex)
            {
                return ex.Status switch
                {
                    ErrorStatus.AlreadyExist => NotFound($"{ex.Status}: {ex.Message}"),
                    ErrorStatus.NotFound => NotFound($"{ex.Status}: {ex.Message}"),
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
                var hotelResponse = _mapper.Map<HotelResponseModel>(await _service.UpdateAsync(id, _mapper.Map<HotelModel>(hotelRequest)));

                return Ok(hotelResponse);
            }
            catch (BusinessException ex)
            {
                return ex.Status switch
                {
                    ErrorStatus.NotFound => NotFound($"{ex.Status}: {ex.Message}"),
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
            catch (BusinessException ex)
            {
                return ex.Status switch
                {
                    ErrorStatus.NotFound => NotFound($"{ex.Status}: {ex.Message}"),
                    _ => BadRequest()
                };
            }
        }
    }
}
