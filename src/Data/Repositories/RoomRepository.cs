using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public IEnumerable<RoomEntity> GetAll(bool asNoTracking = false)
        {
            return asNoTracking ? _rooms.AsNoTracking() : _rooms;
        }

        public async Task<RoomEntity> GetAsync(int id, bool asNoTracking = false)
        {
            return asNoTracking
                ? await _rooms.AsNoTracking().FirstOrDefaultAsync(room => room.Id == id)
                : await _rooms.FirstOrDefaultAsync(room => room.Id == id);
        }

        public IEnumerable<RoomEntity> Find(Func<RoomEntity, bool> predicate, bool asNoTracking = false)
        {
            return asNoTracking ? _rooms.AsNoTracking().Where(predicate) : _rooms.Where(predicate);
        }

        public async Task<RoomEntity> CreateAsync(RoomEntity room)
        {
            var addedRoomEntry = await _rooms.AddAsync(room);
            await _db.SaveChangesAsync();

            return addedRoomEntry.Entity;
        }

        public async Task<RoomEntity> UpdateAsync(RoomEntity newRoom)
        {
            var updatedRoomEntry = _rooms.Update(newRoom);
            await _db.SaveChangesAsync();

            return updatedRoomEntry.Entity;
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
