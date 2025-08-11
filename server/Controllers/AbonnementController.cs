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
    public class AbonnementController : ControllerBase
    {
        private readonly DB_Connect _context;
        public AbonnementController(
            DB_Connect context
        )
        {
            _context = context;
        }

        [HttpGet("paiments")]
        public async Task<IActionResult> GetAbonnementPaiments()
        {
            try
            {
                var paiments = await _context.abonnementPaiments
                    .Include(p => p.Profetionnal)
                    .Select(p => new
                    {
                        p.Id,
                        p.Amount,
                        status = p.Status.ToString(),
                        p.PaymentDate,
                        p.Profetionnal.BusinessName,
                        p.Profetionnal.ProfileImage,
                        p.Profetionnal.User.UserName,
                        p.ProfetionnalId
                    }).ToListAsync();
                ;
                return Ok(paiments);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpGet]

        public async Task<IActionResult> GetAbonnementPaimentsByProfetionnalId()
        {
            try
            {
                var abonnements = await  _context.Abonnements.ToArrayAsync();
                 return Ok(abonnements);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpPost]

        public async Task<IActionResult> NewAbonnement (AbonnementDto dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.Name) || dto.Price <= 0 || dto.DurationValue <= 0)
                {
                    return BadRequest("Invalid data provided.");
                }
                var abonnement = new Abonnements
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                    DurationUnit = dto.DurationUnit,
                    DurationValue = dto.DurationValue,
                   
                };
                _context.Abonnements.Add(abonnement);
                await _context.SaveChangesAsync();
                return Ok(abonnement);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating abonnement: {ex.Message}");
            }
        }


        [HttpPost("paiments")]

        public async Task<IActionResult> NewAbonnementPaiment([FromBody] AbonenmtPaimentDTO dto)
        {
            try
            {
                if (dto.Amount <= 0 || string.IsNullOrEmpty(dto.PaymentMethod) || dto.ProfetionnalId <= 0 || dto.AbonnementId != 0)
                {
                    return BadRequest("Invalid payment data provided.");
                }
                var abonnement = await _context.Abonnements.FindAsync(dto.AbonnementId);
                if (abonnement == null)
                {
                    return NotFound("Abonnement not found.");
                }
                var paiment = new AbonnementPaiment
                {
                    Amount = dto.Amount,
                    PaymentMethod = dto.PaymentMethod,
                    PaymentDate = dto.PaymentDate,
                    ProfetionnalId = dto.ProfetionnalId,
                    TransactionId = dto.TransactionId,
                    Currency = dto.Currency ?? "MAD", // Default to MAD if not provided
                    AbonnementId = dto.AbonnementId,
                    Status = AbonnementPaymentStatus.Paid, // Default status
                };
                _context.abonnementPaiments.Add(paiment);
                await _context.SaveChangesAsync();
                return Ok(paiment);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating payment: {ex.Message}");
            }
        }

    }
}
