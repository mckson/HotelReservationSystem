namespace DataAccessLayer.Entities
{
    class Hotel
    {
        //Primary Key
        public int Id { get; set; }
        public string Name { get; set; }
        //Foreign Key
        public int LocationId { get; set; }
        public int RoomsNumber { get; set; }
        //public List<Room> Rooms { get; set; }
    }
}
