using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOS;
using server.models;
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

        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            try
            {
                var profetionnals = await _context.Profetionnals
     .Include(p => p.User)
     .Include(p => p.Category)
     .Include(p => p.services)
     .Select(p => new ProfetionnalDTO
     {
         id = p.Id,
         Business_name = p.BusinessName,
         Description = p.Description,
         address = p.Address,
         City = p.City,
         Status = p.Status.ToString(), // convert enum to string if needed
          Phone = p.Phone,
          ProfileImage = p.ProfileImage,
         CategoryName = p.Category != null ? p.Category.Name : null,
         category_id = p.Category.Id,
         UserEmail = p.User.Email,
         UserName = p.User.UserName,
         UserPhone = p.User.PhoneNumber,
         TitleService = p.services.FirstOrDefault() != null ? p.services.FirstOrDefault().Title : null,
         ServicePrice = p.services.FirstOrDefault() != null ? p.services.FirstOrDefault().Price : 0


     })
     .ToListAsync();

                if (profetionnals == null || !profetionnals.Any())
                {
                    return NotFound("No professionals found.");
                }
                return Ok(profetionnals);
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




        [HttpPost]
        public async Task<IActionResult> Store([FromForm] fetionnaleCrudDto profetionnalDTO)
        {
            try
            {
                // 1. Vérifie si l'utilisateur existe
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == profetionnalDTO.user_id);
                if (user == null)
                    return NotFound("User not found.");

                // 2. Vérifie si la catégorie existe
                var category = await _context.Categories.FindAsync(profetionnalDTO.category_id);
                if (category == null)
                    return NotFound("Category not found.");

                // 3. Gérer l’upload de l’image
                string? imagePath = null;
                if (profetionnalDTO.ProfileImage is { Length: > 0 })
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "professionals");
                    Directory.CreateDirectory(uploadsFolder); // Crée le dossier s’il n’existe pas

                    var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(profetionnalDTO.ProfileImage.FileName)}";
                    var fullPath = Path.Combine(uploadsFolder, uniqueFileName);

                    using var fileStream = new FileStream(fullPath, FileMode.Create);
                    await profetionnalDTO.ProfileImage.CopyToAsync(fileStream);

                    // Stocke le chemin relatif
                    imagePath = Path.Combine("uploads", "professionals", uniqueFileName).Replace("\\", "/");
                }

                // 4. Création de l’objet
                var profetionnal = new models.Profetionnal
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
                };

                _context.Profetionnals.Add(profetionnal);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Professional created successfully",
                    data = new
                    {
                        profetionnal.Id,
                        profetionnal.BusinessName,
                        profetionnal.ProfileImage
                    }
                });
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




        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var profetionnal = await _context.Profetionnals
                    .Include(p => p.User)
                    .Include(p => p.Category)
                    .Include(p => p.services)
                    .FirstOrDefaultAsync(p => p.Id == id);
                if (profetionnal == null)
                {
                    return NotFound("Professional not found.");
                }
                var profetionnalDTO = new ProfetionnalDTO
                {
                    id = profetionnal.Id,
                    Business_name = profetionnal.BusinessName,
                    Description = profetionnal.Description,
                    address = profetionnal.Address,
                    City = profetionnal.City,
                    Status = profetionnal.Status.ToString(),
                    Phone = profetionnal.Phone,
                    ProfileImage = profetionnal.ProfileImage,
                    CategoryName = profetionnal.Category != null ? profetionnal.Category.Name : null,
                    UserEmail = profetionnal.User.Email,
                    UserName = profetionnal.User.UserName,
                    UserPhone = profetionnal.User.PhoneNumber,
                    TitleService = profetionnal.services.FirstOrDefault()?.Title ?? "No services",
                    ServicePrice = profetionnal.services.FirstOrDefault()?.Price ?? 0
                };

               var  services = await _context.Services
                    .Where(s => s.ProfessionalId == id)
                    .Select(s => new
                    {
                        s.Id,
                        s.Title,
                        s.Description,
                        s.Price,
                        s.Duration,
                    })
                    .ToListAsync();

                var availabilities = await _context.Availabilities
                    .Where(a => a.ProfessionalId == id)
                    .Select(a => new
                    {
                        a.Id,
                        a.DayOfWeek,
                        a.StartTime,
                        a.EndTime
                    })
                    .ToListAsync();
                return Ok(new {profetionnal = profetionnalDTO ,Services = services , availability = availabilities } );
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
