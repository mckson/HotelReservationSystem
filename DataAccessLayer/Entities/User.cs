namespace DataAccessLayer.Entities
{
    class User
    {
        //Primary Key
        public int Id { get; set; }
        //Foreign Key
        public int RoleId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; } //delete later
        //public string Email { get; set; }
    }
}
