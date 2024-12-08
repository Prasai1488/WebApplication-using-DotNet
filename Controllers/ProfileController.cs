using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly JwtService _service;

        public ProfileController(DataContext context, JwtService service)
        {
            _context = context;
            _service = service;
        }

        // Method to get profile details

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                // Get the user ID from token 
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {
                    return Unauthorized(new { Message = "Not authorized." });
                }

                // Finding the user in our database :
                var user = await _context.Users
                    .Where(u => u.Id.ToString() == userId)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return BadRequest(new { Message = "User not found." });
                    
                }

                // now safely return the user profile info :
                var userProfile = new
                {
                    user.Id,
                    user.Username,
                    user.Email,
                };

                return Ok(userProfile);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { Message = "Something unusal happened" });
            }

        }
    }
}
