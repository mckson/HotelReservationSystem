// ReSharper disable once CheckNamespace
namespace HotelReservation.Data.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        
        public int PersonId { get; set; }
        //Related entity
        //public Person Person { get; set; }
        public GuestEntity Person { get; set; }
        
        public int RoleId { get; set; }
        public RoleEntity Role { get; set; }

        public string Login { get; set; }
        public string Password { get; set; } //delete/change later
    }
}
