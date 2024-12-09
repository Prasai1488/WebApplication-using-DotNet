using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTO
{
    public class EditProfileDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
