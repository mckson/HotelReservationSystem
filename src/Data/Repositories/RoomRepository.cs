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
        private readonly DbSet<RoomEntity> _rooms;

        public RoomRepository(HotelContext context)
        {
            _db = context;
            _rooms = context.Rooms;
        }

        public IEnumerable<RoomEntity> GetAll() => _rooms;

        public async Task<IEnumerable<RoomEntity>> GetAllAsync() => await Task.Run(GetAll);

        public RoomEntity Get(int id) => _rooms.FirstOrDefault(room => room.Id == id);

        public async Task<RoomEntity> GetAsync(int id) => await Task.Run(() => Get(id));

        public async Task<RoomEntity> GetAsync(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RoomEntity> Find(Func<RoomEntity, bool> predicate) => _rooms.Where(predicate);

        public async Task<IEnumerable<RoomEntity>> FindAsync(Func<RoomEntity, bool> predicate) => await Task.Run(() => Find(predicate));

        public void Create(RoomEntity room)
        {
            _rooms.Add(room);
            _db.SaveChanges();
        }

        public async Task CreateAsync(RoomEntity room)
        {
            await _rooms.AddAsync(room);
            await _db.SaveChangesAsync();
        }

        // change implementation
        public void Update(RoomEntity newRoom)
        {
            _rooms.Find(newRoom.Id);
            _db.SaveChanges();
        }

        public async Task UpdateAsync(RoomEntity newRoom)
        {
            await _rooms.FindAsync(newRoom.Id);
            await _db.SaveChangesAsync();
        }

        public RoomEntity Delete(int id)
        {
            var deleteRoom = _rooms.Find(id);

            if (deleteRoom != null)
            {
                _rooms.Remove(deleteRoom);
                _db.SaveChanges();
            }

            return deleteRoom;
        }

        public async Task<RoomEntity> DeleteAsync(int id)
        {
            var deleteRoom = await _rooms.FindAsync(id);

            if (deleteRoom != null)
            {
                _rooms.Remove(deleteRoom);
                await _db.SaveChangesAsync();
            }

            return deleteRoom;
        }
    }
}
