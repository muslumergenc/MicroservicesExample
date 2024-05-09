using Microsoft.AspNetCore.Identity;

namespace FreeCourse.IdentityServerApi.Data;

public class ApplicationUser : IdentityUser
{
    public string? City { get; set; }
}