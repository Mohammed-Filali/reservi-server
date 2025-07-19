using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.Data;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatrgoriesController : ControllerBase
    {
        private readonly DB_Connect _context;

        public CatrgoriesController(DB_Connect context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<IActionResult> index()
        {
            try
            {
                var categories =  _context.Categories.ToArray();
                return Ok(categories);
            }catch (Exception ex)
             {
            return  BadRequest(ex);
             }
        }
    }
}
