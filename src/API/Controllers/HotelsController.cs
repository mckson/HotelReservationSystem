using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Data;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Query.Internal;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HotelReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly HotelContext _db;
        private readonly HotelRepository _hoteRepo;

        public HotelsController(HotelContext context)
        {
            _db = context;
            _hoteRepo = new HotelRepository(_db);
        }

        // GET: api/<HotelsController>
        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<HotelDTO>> Get()
        {
            var hotels = await _hoteRepo.GetAllAsync();
            var hotelsDTO =  hotels
                .Select(h => new HotelDTO
                {
                    Id = h.Id,
                    Name = h.Name,
                    Rooms = h.Rooms.Select(r => new RoomDTO
                    {
                        HotelName = r.Hotel.Name,
                        Number = r.RoomNumber
                    })
                });

            return hotelsDTO;
        }

        // GET api/<HotelsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelDTO>> Get(int id)
        {
            var hotel = await _hoteRepo.GetAsync(id);
            try
            {
                if (hotel == null) throw new Exception();
                var hotelDto = new HotelDTO
                {
                    Id = hotel.Id,
                    Name = hotel.Name,
                    Rooms = hotel.Rooms.Select(r => new RoomDTO
                    {
                        HotelName = r.Hotel.Name,
                        Number = r.RoomNumber
                    })

                };

                return Ok(hotelDto);
            }
            catch
            {
                return NotFound();
            }
        }

        // POST api/<HotelsController>
        [HttpPost]
        public async Task<ActionResult<HotelEntity>> Post([FromBody] HotelEntity hotel)
        {
            await _hoteRepo.CreateAsync(hotel);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new {id = hotel.Id}, hotel);
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

    //test class
    public class HotelDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<RoomDTO> Rooms { get; set; }
        public LocationEntity Location { get; set; }
    }

    //test classs
    public class RoomDTO
    {
        public int Number { get; set; }
        public string HotelName { get; set; }
    }
}
