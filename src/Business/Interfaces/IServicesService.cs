using System.Collections.Generic;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;

namespace HotelReservation.Business.Interfaces
{
    public interface IServicesService : IBaseService<ServiceEntity, ServiceModel>
    {
        IEnumerable<ServiceModel> GetAllServices();
    }
}
