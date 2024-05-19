using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace DACS.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public byte? Gender { get; set; }

        public List<PhieuLayMau>? PhieuLayMau { get; set; }
    }
}
