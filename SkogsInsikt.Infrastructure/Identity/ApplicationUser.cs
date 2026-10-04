using Microsoft.AspNetCore.Identity;

namespace SkogsInsikt.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}
