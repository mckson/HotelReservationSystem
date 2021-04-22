using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Person))]
        public int PersonId { get; set; }
        //Related entity
        //public Person Person { get; set; }
        public Guest Person { get; set; }

        [ForeignKey(nameof(Role))]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public string Login { get; set; }
        public string Password { get; set; } //delete/change later
    }
}
