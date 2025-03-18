using System;
using Microsoft.AspNetCore.Identity;

namespace ABB.Domain.IdentityModels
{

public class AppRole : IdentityRole<string>
{
    public AppRole()
    {
        this.Id = Guid.NewGuid().ToString("N");
    }
    public int RoleCode { get; set; }
    public string Description { get; set; }

    public bool IsDeleted { get; set; }
}
}