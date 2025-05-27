using System;
using Microsoft.AspNetCore.Identity;

namespace ABB.Domain.IdentityModels
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            Id = Guid.NewGuid().ToString("N");
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public string Photo { get; set; }
        public string Address { get; set; }
        public DateTime? PasswordExpiredDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        public string Signature { get; set; }

        public string Jabatan { get; set; }
    }
}