using System.Linq;

namespace HotelReservation.Data.Interfaces
{
    public interface ISortHelper<TEntity>
    {
        IQueryable<TEntity> ApplySort(IQueryable<TEntity> entities, string orderByPropertyName, bool isDescending);
    }
}
