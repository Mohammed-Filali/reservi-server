using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.Data;
using server.Repositories;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminServices _adminServices;
        
        private readonly DB_Connect _context;
        public AdminController(
            IAdminServices adminServices,
            DB_Connect context
            ) 
        {
            _adminServices = adminServices;
            _context = context;
        }

        [HttpGet("dashboard")]

        public async Task<IActionResult> AdminDashboard()
        {
            try
            {
                var profetionnals = await _adminServices.ProfetionnalsCount();
                var clients = await _adminServices.ClientsCount();
                return Ok(new
                {
                    ProfetionnalsCount = profetionnals,
                    ClientsCount = clients
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }
    }
}
