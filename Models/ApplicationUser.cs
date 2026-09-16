using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace shipnet.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string NombreCompleto { get; set; } = string.Empty;
    }
}