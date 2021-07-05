using HotelReservation.Data.Interfaces;
using System;

namespace HotelReservation.Data.Entities
{
    public abstract class Entity : IEntity
    {
        public Guid Id { get; set; }
    }
}
