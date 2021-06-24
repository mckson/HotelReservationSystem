using AutoMapper;
using HotelReservation.API.Helpers;
using HotelReservation.API.Models.RequestModels;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Constants;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Data.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelsService _hotelsService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public HotelsController(IHotelsService hotelsService, IMapper mapper, IUriService uriService)
        {
            _hotelsService = hotelsService;
            _uriService = uriService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<BasePagedResponseModel<HotelResponseModel>> GetHotelsAsync([FromQuery] PaginationFilter paginationFilter, [FromQuery] HotelsFilter hotelsFilter)
        {
            var route = Request.Path.Value;

            var validatedFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize.Value);

            var hotelsModel =
                await _hotelsService.GetPagedHotelsAsync(validatedFilter, hotelsFilter);
            var totalHotels = await _hotelsService.GetCountAsync(hotelsFilter);

            var hotelsResponse = _mapper.Map<IEnumerable<HotelResponseModel>>(hotelsModel);

            var pagedHotelsResponse = PaginationHelper.CreatePagedResponseModel(
                hotelsResponse,
                validatedFilter,
                totalHotels,
                _uriService,
                route);

            return pagedHotelsResponse;
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<HotelResponseModel>> GetHotelByIdAsync(int id)
        {
            var hotelModel = await _hotelsService.GetAsync(id);
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
            var hotelResponse = _mapper.Map<HotelResponseModel>(await _hotelsService.GetHotelByNameAsync(name));

            return Ok(hotelResponse);
        }

        [Authorize(Policy = Policies.AdminPermission)]
        [HttpPost]
        public async Task<ActionResult<HotelResponseModel>> CreateHotelAsync([FromBody] HotelRequestModel hotelRequest)
        {
            var hotelModel = _mapper.Map<HotelModel>(hotelRequest);
            hotelModel.HotelUsers = new List<HotelUserModel>();

            if (hotelRequest.Managers != null)
            {
                hotelModel.HotelUsers.AddRange(hotelRequest.Managers
                    .Select(manager => new HotelUserModel { UserId = manager }).ToList());
            }

            var createdHotel = await _hotelsService.CreateAsync(hotelModel);

            var hotelResponse =
                _mapper.Map<HotelResponseModel>(createdHotel);

            return Ok(hotelResponse);
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = Policies.AdminPermission)]
        public async Task<ActionResult<HotelResponseModel>> UpdateHotelAsync(int id, [FromBody] HotelRequestModel hotelRequest)
        {
            var hotelModel = _mapper.Map<HotelModel>(hotelRequest);
            hotelModel.HotelUsers = new List<HotelUserModel>();

            if (hotelRequest.Managers != null)
            {
                hotelModel.HotelUsers.AddRange(hotelRequest.Managers
                    .Select(manager => new HotelUserModel { UserId = manager }).ToList());
            }

            var updatedHotel = await _hotelsService.UpdateAsync(
                id,
                hotelModel);

            var hotelResponse =
                _mapper.Map<HotelResponseModel>(updatedHotel);

            return Ok(hotelResponse);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = Policies.AdminPermission)]
        public async Task<ActionResult<HotelResponseModel>> DeleteHotelAsync(int id)
        {
            var deletedHotelModel = await _hotelsService.DeleteAsync(id);
            var deletedHotelResponse = _mapper.Map<HotelResponseModel>(deletedHotelModel);
            return Ok(deletedHotelResponse);
        }
    }
}
