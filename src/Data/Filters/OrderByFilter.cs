namespace HotelReservation.Data.Filters
{
    public abstract class OrderByFilter
    {
        public string PropertyName { get; set; }

        public bool IsDescending { get; set; }
    }
}
