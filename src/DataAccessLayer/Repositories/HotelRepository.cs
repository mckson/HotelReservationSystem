using System;
using System.Collections.Generic;
using System.Linq;
using HotelReservation.Data;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class HotelRepository : IRepository<HotelEntity>
    {
        private HotelContext db;

        public HotelRepository(HotelContext context)
        {
            db = context;
        }

        public IEnumerable<HotelEntity> GetAll()
        {
            return db.Hotels;
        }
        public HotelEntity Get(int id)
        {
            return db.Hotels.Find(id);
        }

        public void Create(HotelEntity hotel)
        {
            db.Hotels.Add(hotel);
        }

        public void Update(HotelEntity hotel)
        {
            db.Entry(hotel).State = EntityState.Modified;
        }

        public IEnumerable<HotelEntity> Find(Func<HotelEntity, bool> predicate)
        {
            return db.Hotels.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            var hotel = db.Hotels.Find(id);

            if (hotel != null)
                db.Hotels.Remove(hotel);
        }
    }
}
