using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOS;
using server.Models;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfetionnalsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DB_Connect _context;
        public ProfetionnalsController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            DB_Connect context
            )
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]

        [HttpPost("store")]
        public async Task<IActionResult> Store([FromForm] ProfetionnalDTO profetionnalDTO)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == profetionnalDTO.user_id);
                if (user == null)
                    return NotFound("User not found.");

                var category = await _context.Categories.FindAsync(profetionnalDTO.category_id);
                if (category == null)
                    return NotFound("Category not found.");

                // Handle image upload
                string? imagePath = null;
                if (profetionnalDTO.ProfileImage != null && profetionnalDTO.ProfileImage.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    Directory.CreateDirectory(uploadsFolder); // Ensure folder exists

                    var uniqueFileName = Guid.NewGuid() + Path.GetExtension(profetionnalDTO.ProfileImage.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await profetionnalDTO.ProfileImage.CopyToAsync(fileStream);
                    }

                    imagePath = Path.Combine("uploads", uniqueFileName).Replace("\\", "/");
                }

                var profetionnal = new Profetionnal
                {
                    BusinessName = profetionnalDTO.Business_name,
                    Address = profetionnalDTO.address,
                    Description = profetionnalDTO.Description,
                    City = profetionnalDTO.City,
                    UserId = profetionnalDTO.user_id,
                    Phone = user.PhoneNumber,
                    CategoryId = profetionnalDTO.category_id,
                    ProfileImage = imagePath,
                    Status = ProfetionnalStatus.Active,
                    User = user,
                    Category = category,
                   
                };

                _context.Profetionnals.Add(profetionnal);
                await _context.SaveChangesAsync();

                return Ok(profetionnalDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Internal Server Error",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }

        }

    }
}
