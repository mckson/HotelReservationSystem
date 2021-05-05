namespace HotelReservation.Business.Interfaces
{
    public interface IEntityModelMapper<TEntity, TModel>
    {
        public TModel EntityToModel(TEntity entity);

        public TEntity ModelToEntity(TModel model);
    }
}
