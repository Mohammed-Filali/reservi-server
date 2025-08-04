using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOS;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly DB_Connect _context;

        public ClientsController( DB_Connect context  )
        {
            _context = context;
        }

        [HttpGet]

        public async Task<IActionResult> GetClients()
        {
            // On récupère les utilisateurs avec le rôle "Client"
            var clientRole = await _context.Roles
                .Where(r => r.Name == "client")
                .FirstOrDefaultAsync();

            if (clientRole == null)
                return NotFound("Role 'Client' not found");

            var clientIds = await _context.UserRoles
                .Where(ur => ur.RoleId == clientRole.Id)
                .Select(ur => ur.UserId)
                .ToListAsync();

            var clients = await _context.Users
                .Where(u => clientIds.Contains(u.Id))
                .ToListAsync();
        
            return Ok(clients);
        }


    }
}
