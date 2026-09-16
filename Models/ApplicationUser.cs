using Microsoft.AspNetCore.Identity;

namespace shipnet.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string NombreCompleto { get; set; } = string.Empty;
    }
}