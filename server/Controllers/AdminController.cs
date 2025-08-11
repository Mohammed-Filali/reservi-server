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
                var reservation = await _adminServices.ReservationsCountThisMonth();
                var cancelingPayment = await _adminServices.CancelingPayment();
                var pendingPayment = await _adminServices.PendingPayment();
                var totalRevenue = await _adminServices.TotalRevenue();

                return Ok(new
                {
                    ProfetionnalsCount = profetionnals,
                    ClientsCount = clients,
                    ReservationCount = reservation,
                    CancelingPayment = cancelingPayment,
                    PendingPayment = pendingPayment,
                    TotalRevenue = totalRevenue

                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }
    }
}
