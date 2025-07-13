using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server.Models;
using System;
using System.Threading.Tasks;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeederController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public SeederController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> SeedRolesAndAdmin()
        {
            string[] roleNames = { "admin", "client", "professionnel" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!roleResult.Succeeded)
                    {
                        return BadRequest($"Failed to create role {roleName}: {string.Join(", ", roleResult.Errors)}");
                    }
                }
            }

            var adminEmail = "admin@gmail.com";
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var userResult = await _userManager.CreateAsync(admin, "Admin@123"); // strong password

                if (!userResult.Succeeded)
                {
                    return BadRequest($"Failed to create admin user: {string.Join(", ", userResult.Errors)}");
                }

                var addRoleResult = await _userManager.AddToRoleAsync(admin, "admin");

                if (!addRoleResult.Succeeded)
                {
                    return BadRequest($"Failed to assign admin role: {string.Join(", ", addRoleResult.Errors)}");
                }
            }

            return Ok("Seeding roles and admin user completed successfully.");
        }
    }
}
