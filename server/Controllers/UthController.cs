using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data; // Your DbContext namespace
using server.DTOS;
using server.Models;
using server.Repositories;
using System.Security.Claims;

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
        private readonly IPaimentService _paimentService;
        private readonly DB_Connect _context;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            DB_Connect context,            
            ITokenService tokenService,
            IConfiguration configuration,
            IPaimentService paimentService
            )
        {
            _configuration = configuration;
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _paimentService = paimentService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UsersDTO userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check role exists
            var role = await _roleManager.FindByIdAsync(userDto.Role);
            if (role == null)
            {
                return BadRequest($"Role with id {userDto.Role} does not exist.");
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
            var roles = await _userManager.GetRolesAsync(user);
            var primaryRole = roles.FirstOrDefault(); // Prend le premier rôle s'il y en a plusieurs

            var data = new UsersDTO
            {
                
                Email = user.Email,
                Name = user.UserName,
                Phone = user.PhoneNumber,
                Role = primaryRole
            };
            return Ok(new {user = data , id = user.Id});
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleManager.Roles
                .Where(r => r.Name != "admin")
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
                return Unauthorized(login.Email);

            var passwordValid = await _userManager.CheckPasswordAsync(user, login.Password);
            if (!passwordValid)
                return Unauthorized("Invalid username or password.");

            var token = await _tokenService.GenerateTokenAsync(user);

            // Lire l'expiration du token depuis la config
            var expirationMinutes = double.Parse(_configuration["Jwt:ExpiresInMinutes"]);
            var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

            // Récupérer les rôles de l'utilisateur
            var roles = await _userManager.GetRolesAsync(user);
            var primaryRole = roles.FirstOrDefault(); // Prend le premier rôle s'il y en a plusieurs

            var data = new UsersDTO
            {
                Email = user.Email,
                Name = user.UserName,
                Phone = user.PhoneNumber,
                Role = primaryRole
            };
            if (primaryRole == "professionnel")
            {
                int pro = _context.Profetionnals
                    .Where(p => p.UserId == user.Id)
                    .Select(p =>  p.Id)
                    .FirstOrDefault();
                bool isAbonnementPayed = await _paimentService.isAbonnementPayed(pro);
                return Ok(new
                {
                    user = data,
                    Token = token,
                    ExpiresAt = expiresAt,
                     isAbonnementPayed
                });
            }
            return Ok(new
            {
                user = data,
                Token = token,
                ExpiresAt = expiresAt
            });
        }


        [HttpGet("me")]
        [Authorize] // Requires authentication
        public async Task<IActionResult> GetCurrentUser()
        {
            // Get the user ID from the current ClaimsPrincipal
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            // Find user by ID
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            // Get roles of the user
            var roles = await _userManager.GetRolesAsync(user);
            var primaryRole = roles.FirstOrDefault();
            var userDto = new UsersDTO
            {
                Email = user.Email,
                Name = user.UserName,
                Phone = user.PhoneNumber,
                Role = primaryRole
            };

            // Prepare DTO to return
            if (primaryRole == "professionnel")
            {
                var profetionnale =   _context.Profetionnals.Where(p=> p.UserId == user.Id).FirstOrDefault();
                bool isAbonnementPayed = await _paimentService.isAbonnementPayed(profetionnale.Id);
                var pro = new ProfetionnalDTO
                {
                    id = profetionnale.Id,
                    address = profetionnale.Address,
                    Business_name = profetionnale.BusinessName,
                    City = profetionnale.City,
                    Description = profetionnale.Description,
                    ProfileImage = profetionnale.ProfileImage,


                };
                return Ok(new{ user = userDto , profetionnal =pro , isAbonnementPayed });
            }

           

            return Ok(new { user = userDto , id = user.Id });
        }




        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            return Ok(new { message = "Logout successful" });
        }



    }
}
