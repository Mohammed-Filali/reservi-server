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
    public class AvailabilitiesController : ControllerBase
    {
        private readonly DB_Connect _context;
        private readonly ILogger<AvailabilitiesController> _logger;

        public AvailabilitiesController(DB_Connect context, ILogger<AvailabilitiesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("{proID}")]
        public async Task<IActionResult> GetAvailabilities(int proID)
        {
            try
            {
                var availabilities = await _context.Availabilities
                    .Where(a => a.ProfessionalId == proID)
                    .ToListAsync();

                return Ok(availabilities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving availabilities for professional {ProfessionalId}", proID);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving availabilities");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAvailability(int id, [FromBody] AvaiblitiesDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var availability = await _context.Availabilities
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (availability == null)
                {
                    return NotFound($"Availability with ID {id} not found.");
                }

                // Update fields
                availability.DayOfWeek = dto.Day;
                availability.StartTime = dto.Start;
                availability.EndTime = dto.End;
                availability.ProfessionalId = dto.ProfessionalID;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Updated availability with ID {AvailabilityId}", id);
                return Ok(availability);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating availability with ID {AvailabilityId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the availability");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateAvailability([FromBody] AvaiblitiesDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Find existing availability for the same day and professional
                var existingAvailability = await _context.Availabilities
                    .FirstOrDefaultAsync(a => a.DayOfWeek == dto.Day && a.ProfessionalId == dto.ProfessionalID);

                if (existingAvailability != null)
                {
                    // Update existing availability
                    existingAvailability.StartTime = dto.Start;
                    existingAvailability.EndTime = dto.End;

                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Updated existing availability with ID {AvailabilityId}", existingAvailability.Id);
                    return Ok(existingAvailability);
                }

                // Create new availability
                var newAvailability = new Availability
                {
                    DayOfWeek = dto.Day,
                    StartTime = dto.Start,
                    EndTime = dto.End,
                    ProfessionalId = dto.ProfessionalID
                };

                _context.Availabilities.Add(newAvailability);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Created new availability with ID {AvailabilityId}", newAvailability.Id);
                return CreatedAtAction(nameof(GetAvailabilities), new { proID = dto.ProfessionalID }, newAvailability);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating or updating availability for professional {ProfessionalId} on day {Day}",
                    dto.ProfessionalID, dto.Day);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the availability");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAvailability(int id)
        {
            try
            {
                var availability = await _context.Availabilities
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (availability == null)
                {
                    return NotFound($"Availability with ID {id} not found.");
                }

                _context.Availabilities.Remove(availability);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Deleted availability with ID {AvailabilityId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting availability with ID {AvailabilityId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the availability");
            }
        }
    }
}