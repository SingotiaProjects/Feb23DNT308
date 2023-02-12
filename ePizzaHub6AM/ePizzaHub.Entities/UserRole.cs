using System.ComponentModel.DataAnnotations.Schema;

namespace ePizzaHub.Entities
{
    public class UserRole
    {
        //FK
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        //FK
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
