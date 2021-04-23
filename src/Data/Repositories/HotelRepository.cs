using System;
using System.Collections.Generic;
using System.Linq;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;

namespace HotelReservation.Data.Repositories
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

        public void Update(HotelEntity newHotel)
        {
            var oldHotel = db.Hotels.Find(newHotel.Id);
            oldHotel = newHotel;
        }

        public IEnumerable<HotelEntity> Find(Func<HotelEntity, bool> predicate)
        {
            return db.Hotels.Where(predicate);
        }

        public void Delete(int id)
        {
            var hotel = db.Hotels.Find(id);

            if (hotel != null)
                db.Hotels.Remove(hotel);
        }
    }
}
