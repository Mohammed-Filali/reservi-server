using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.DTOS;
using server.Models;
using server.Data; // Your DbContext namespace

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        private readonly DB_Connect _context;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            DB_Connect context,            
            ITokenService tokenService,
            IConfiguration configuration
            )
        {
            _configuration = configuration;
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UsersDTO userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check role exists
            var role = await _roleManager.FindByIdAsync(userDto.Role_id);
            if (role == null)
            {
                return BadRequest($"Role with id {userDto.Role_id} does not exist.");
            }

            var user = new ApplicationUser
            {
                UserName = userDto.Name,
                Email = userDto.Email,
                PhoneNumber = userDto.Phone
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(error.Code, error.Description);
                return BadRequest(ModelState);
            }

            // Create RoleUser link manually
            var userRole = new IdentityUserRole<string>
            {
                UserId = user.Id,
                RoleId = role.Id
            };

             _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User registered and role assigned successfully." });
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _context.Roles
                .Where(r => r.Name.ToLower() != "admin")
                .ToListAsync();

            return Ok(roles);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
                return Unauthorized("Invalid username or password.");

            var passwordValid = await _userManager.CheckPasswordAsync(user, login.Password);
            if (!passwordValid)
                return Unauthorized("Invalid username or password.");

            var token = await _tokenService.GenerateTokenAsync(user);

            // Read expiration from config
            var expirationMinutes = double.Parse(_configuration["Jwt:ExpiresInMinutes"]);
            var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

            return Ok(new
            {
                token,
                expiresAt
            });
        }

    }
}
