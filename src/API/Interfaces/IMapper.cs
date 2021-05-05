namespace HotelReservation.API.Interfaces
{
    public interface IMapper<TEntityModel, TResponseModel, TRequestModel>
    {
        public TResponseModel EntityToResponse(TEntityModel entityModel);

        public TResponseModel RequestToResponse(TRequestModel requestModel);

        public TEntityModel RequestToEntity(TRequestModel requestModel);
    }
}
