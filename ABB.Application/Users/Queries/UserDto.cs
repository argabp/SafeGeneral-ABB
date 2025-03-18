namespace ABB.Application.Users.Queries
{
    public class UserDto
    {
        public string Id { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string LeaderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public string Photo { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public bool LockoutEnabled { get; set; }
        public bool IsDeleted { get; set; }
    }
}