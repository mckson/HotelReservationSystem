namespace DataAccessLayer.Entities
{
    class Role
    {
        //Primary Key
        public int Id { get; set; }
        public string RoleName { get; set; }
        //Foreign Key
        public int PermissionsId { get; set; }
    }
}
