using Microsoft.AspNetCore.Identity;

namespace ShipNetApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string NombreCompleto { get; set; } = string.Empty;
    }
}