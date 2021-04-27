using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
    public class RoomRepository : IRepository<RoomEntity>
    {
        private readonly HotelContext _db;

        public RoomRepository(HotelContext context)
        {
            _db = context;
        }

        public IEnumerable<RoomEntity> GetAll()
        {
            return _db.Rooms
                /*.Include(room => room.Hotel)
                .Include(room => room.Reservation)
                .Include(room => room.Guest)*/;
        }

        public async Task<IEnumerable<RoomEntity>> GetAllAsync()
        {
            return await Task.Run(GetAll);
        }

        public RoomEntity Get(int id)
        {
            return _db.Rooms
                /*.Include(room => room.Hotel)
                .Include(room => room.Reservation)
                .Include(room => room.Guest)*/
                .FirstOrDefault(room => room.Id == id);
        }

        public async Task<RoomEntity> GetAsync(int id)
        {
            return await Task.Run(() => Get(id));
        }

        public IEnumerable<RoomEntity> Find(Func<RoomEntity, bool> predicate)
        {
            return _db.Rooms
                /*.Include(room => room.Hotel)
                .Include(room => room.Reservation)
                .Include(room => room.Guest)*/
                .Where(predicate);
        }

        public async Task<IEnumerable<RoomEntity>> FindAsync(Func<RoomEntity, bool> predicate)
        {
            return await Task.Run(() => Find(predicate));
        }

        public void Create(RoomEntity room)
        {
            _db.Rooms.Add(room);
        }

        public async Task CreateAsync(RoomEntity room)
        {
            await _db.Rooms.AddAsync(room);
        }

        //change implementation
        public void Update(RoomEntity newRoom)
        {
            var oldRoom = _db.Rooms.Find(newRoom.Id);
            oldRoom = newRoom;
        }

        public async Task UpdateAsync(RoomEntity newRoom)
        {
            var oldRoom = await _db.Rooms.FindAsync(newRoom.Id);
            oldRoom = newRoom;
        }

        public void Delete(int id)
        {
            var deleteRoom = _db.Rooms.Find(id);
            if (deleteRoom != null)
                _db.Rooms.Remove(deleteRoom);
        }

        public async Task DeleteAsync(int id)
        {
            var deleteRoom = await _db.Rooms.FindAsync(id);
            if (deleteRoom != null)
                _db.Rooms.Remove(deleteRoom);
        }
    }
}
