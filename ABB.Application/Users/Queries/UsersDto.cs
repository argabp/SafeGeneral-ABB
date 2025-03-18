using System.ComponentModel;

namespace ABB.Application.Users.Queries
{
    public class UsersDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string LeaderName { get; set; }
        public string RoleId { get; set; }
        public string Role { get; set; }
        public string LeaderId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string PasswordExpiryDate { get; set; }

        [DisplayName("Status")]
        public bool IsActive { get; set; } = true;

        [DisplayName("Locked Out")]
        public bool LockoutEnabled { get; set; } = true;

        public string Photo { get; set; }
    }
}