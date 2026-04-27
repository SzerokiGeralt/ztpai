using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ztpai.Models;

namespace ztpai.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest(new { error = "Email and password are required" });

            var user = new User { UserName = req.Email, Email = req.Email };
            var result = await _userManager.CreateAsync(user, req.Password);

            if (!result.Succeeded)
                return BadRequest(new { error = string.Join(", ", result.Errors.Select(e => e.Description)) });

            // Przypisz rolę "user" przy rejestracji
            await _userManager.AddToRoleAsync(user, "user");

            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var user = await _userManager.FindByEmailAsync(req.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, req.Password))
                return Unauthorized(new { error = "Invalid email or password" });

            var roles = await _userManager.GetRolesAsync(user);
            var token = GenerateToken(user.Email, roles);

            return Ok(new { token });
        }

        private string GenerateToken(string email, IList<string> roles)
        {
            var jwtKey = _configuration["JwtKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(handler.CreateToken(tokenDescriptor));
        }
    }

    public class RegisterRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
