using System;
using System.Collections.Generic;

namespace HotelReservation.Data.Interfaces
{
    interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        IEnumerable<T> Find(Func<T, Boolean> predicate);
        void Create(T item);
        void Update(T newHotel);
        void Delete(int id);
    }
}
