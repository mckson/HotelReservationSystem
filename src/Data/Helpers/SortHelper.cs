using Castle.Core.Internal;
using HotelReservation.Data.Interfaces;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace HotelReservation.Data.Helpers
{
    public class SortHelper<TEntity> : ISortHelper<TEntity>
    {
        public IQueryable<TEntity> ApplySort(
            IQueryable<TEntity> entities,
            string orderByPropertyName,
            bool isDescending)
        {
            if (!entities.Any())
            {
                return entities;
            }

            if (orderByPropertyName.IsNullOrEmpty())
            {
                return entities;
            }

            var sortingOrder = isDescending ? "descending" : "ascending";

            var orderQuery = $"{orderByPropertyName} {sortingOrder}";

            return entities.OrderBy(orderQuery);
        }
    }
}
