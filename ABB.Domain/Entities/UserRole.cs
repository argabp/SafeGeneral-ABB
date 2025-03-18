using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABB.Domain.Entities
{
    [Table("TR_UserRole")]
    public class UserRole
    {
        [Key] public string UserId { get; set; }

        public string RoleId { get; set; }
    }
}