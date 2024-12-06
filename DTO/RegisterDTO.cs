using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTO
{
    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set;}

        [Required]
        [MinLength(6, ErrorMessage ="Password should be atleast 6 characters long.")]
        public string Password { get; set;}

    }
}
