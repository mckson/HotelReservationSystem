using System;
using System.Collections.Generic;
using System.Linq;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;

namespace HotelReservation.Data.Repositories
{
    public class RoomRepository : IRepository<RoomEntity>
    {
        private HotelContext db;

        public RoomRepository(HotelContext context)
        {
            db = context;
        }

        public IEnumerable<RoomEntity> GetAll()
        {
            return db.Rooms;
        }

        public RoomEntity Get(int id)
        {
            return db.Rooms.Find(id);
        }

        public IEnumerable<RoomEntity> Find(Func<RoomEntity, bool> predicate)
        {
            return db.Rooms.Where(predicate);
        }

        public void Create(RoomEntity room)
        {
            db.Rooms.Add(room);
        }

        public void Update(RoomEntity newRoom)
        {
            var oldRoom = db.Rooms.Find(newRoom.Id);
            oldRoom = newRoom;
        }

        public void Delete(int id)
        {
            var deleteRoom = db.Rooms.Find(id);
            if (deleteRoom != null)
                db.Rooms.Remove(deleteRoom);
        }
    }
}
