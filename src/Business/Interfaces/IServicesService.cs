using HotelReservation.Business.Models;
using System.Collections.Generic;

namespace HotelReservation.Business.Interfaces
{
    public interface IServicesService : IBaseService<ServiceModel>, IUpdateService<ServiceModel>
    {
        IEnumerable<ServiceModel> GetAllServices();
    }
}
