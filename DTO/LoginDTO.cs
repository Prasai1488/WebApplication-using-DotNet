using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTO
{
    public class LoginDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
