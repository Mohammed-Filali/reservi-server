using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOS;
using server.Models;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly DB_Connect _context;

        public ServicesController(DB_Connect context )
        {
            _context = context;
        }

        [HttpGet]

        public async Task<IActionResult> index()
        {
            var services = _context.Services
               .Include(s => s.Professional)
             .Select(s => new {
           s.Id,
           s.Title,
           s.Description,
           s.Duration,
           s.Price,
           Professional = new
           {
               s.Professional.Id,
               s.Professional.BusinessName
           }
       }).ToList();

            return Ok(services);

        }

        [HttpGet("{ProID}")]

        public async Task<IActionResult> MyServices(int ProID)
        {
            var services = _context.Services
                .Where(s=> s.ProfessionalId == ProID)
                .ToList();

            return Ok(services);

        }


        [HttpPost]

        public async Task<IActionResult> store(ServicesDTO servicesDTO) 
        {
            try
            {
                Service service = new Service
                {
                    ProfessionalId = servicesDTO.Professional,
                    Description = servicesDTO.DescriptionService,
                    Price = servicesDTO.Price,
                    Title = servicesDTO.TitleService,
                    Duration = servicesDTO.DurationService,

                };
                _context.Services.Add(service);
                await _context.SaveChangesAsync();
                return Ok(servicesDTO);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;

                if (ex.InnerException != null)
                {
                    errorMessage += " | InnerException: " + ex.InnerException.Message;

                    if (ex.InnerException.InnerException != null)
                        errorMessage += " | DeepInnerException: " + ex.InnerException.InnerException.Message;
                }

                return BadRequest(errorMessage);
            }



        }


        [HttpPut("{id}")]
        public async Task<IActionResult> update( ServicesDTO dto ,  int id)
        {
            try
            {
                var service =  _context.Services
                    .SingleOrDefault(s=> s.Id == id);
                if (service == null){
                    return BadRequest("ther is no service with this id");
                        }

                service.Title = dto.TitleService;
                service.Description = dto.DescriptionService;
                service.Price = dto.Price;
                service.Duration = dto.DurationService;
                service.ProfessionalId = dto.Professional;
        

                await _context.SaveChangesAsync();
                return Ok(service);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                return BadRequest(errorMessage);
            }
        }


        [HttpDelete("{id}")]

        public async Task<IActionResult> delete(int id)
        {
            try
            {
                var service = _context.Services
                    .SingleOrDefault(s => s.Id == id);

                if (service == null)
                {
                    return BadRequest("ther is no service with htis id");
                }

                _context.Remove(service);
                await _context.SaveChangesAsync();
                return Ok(service);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }
        
    }
}
