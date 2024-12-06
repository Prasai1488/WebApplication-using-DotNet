using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.DTO;
using Microsoft.EntityFrameworkCore;


namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly DataContext _context;
        private readonly JwtService _jwtService;

        public AuthController(DataContext context,JwtService jwtService)
        {
            this._context = context;
            this._jwtService = jwtService;
        }

        // register api endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            try
            {
                // Validate input:
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // checking for duplicate username or email:
                if (_context.Users.Any(u => u.Email == registerDto.Email || u.Username == registerDto.Username))
                    return BadRequest(new {Message = "Fuck off"});

                // now lets create new user
                var user = new User
                {
                    Email = registerDto.Email,
                    Username = registerDto.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return StatusCode(201, new { Message = "New user created successfully" });


            }

            catch (DbUpdateException ex)
            {
               
          
                return StatusCode(500, new { Message = "Database error occurred." });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred. Please try again." });
            }
        }

        // Login API
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username);

                if (user == null)
                
                    return Unauthorized(new { Message = "Invalid credentials" });

                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                    return Unauthorized(new { Message = "Invalid credentials" });

                // Now lets generate jwt token
                var token = _jwtService.GenerateJwtToken(user);

                return Ok(new {Message = "Login Success", Token = token });

            } 
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something unusal happened " });
            }
        }

    }

   
}
