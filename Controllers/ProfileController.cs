using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication1.DTO;
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

                if (string.IsNullOrEmpty(userId))
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


        // Method to edit user profile :
        [HttpPut("profile")]
        [Authorize]

        public async Task<IActionResult> UpdateProfile([FromBody] EditProfileDTO editProfileDTO)
        {
            try
            {
                // Get the user ID from token 
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
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

                // Checking if email or username is already taken by another user 
                if (_context.Users.Any(u => (u.Email == editProfileDTO.Email || u.Username == editProfileDTO.Username) && u.Id != user.Id))
                {
                    return BadRequest(new { Message = "Username or Email is already taken." });
                }

                // Update user email and username with new info:
                user.Email = editProfileDTO.Email;
                user.Username = editProfileDTO.Username;

                await _context.SaveChangesAsync();

                var updatedProfile = new
                {
                    user.Id,
                    user.Email,
                    user.Username,
                };

                return Ok(updatedProfile);

            }
            catch(Exception ex)
            {
                return StatusCode(500, new { Message = "Something unusual happened" });
            }
        }

        // Method to delete user profile :
        [HttpDelete("profile")]
        [Authorize]

        public async Task<IActionResult> DeleteProfile()
        {
            try
            {
                // Extract user id from token
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { Message = "Not authorized" });
                }

                // Finding user in the database :
                var user = await _context.Users.Where(u => u.Id.ToString() == userId).FirstOrDefaultAsync();

                if (user == null)
                {
                    return BadRequest(new { Message = "User not found" });
                }

                // Delete the user
                 _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "User deleted successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Something unusal happened" });
            }



        }

    }
}
