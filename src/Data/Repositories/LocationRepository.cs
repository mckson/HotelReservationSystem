using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservation.Data.Repositories
{
    public class LocationRepository : BaseRepository<LocationEntity>, ILocationRepository
    {
        public LocationRepository(HotelContext context)
            : base(context)
        {
        }

        public async Task<LocationEntity> GetAsync(
            string country,
            string region,
            string city,
            string street,
            int building)
        {
            return await DbSet.FirstOrDefaultAsync(location =>
                location.Country == country &&
                location.Region == region &&
                location.City == city &&
                location.Street == street &&
                location.BuildingNumber == building);
        }
    }
}
