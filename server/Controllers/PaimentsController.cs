using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOS;
using server.Models;
using System.Threading.Tasks;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaimentsController : ControllerBase
    {
        private readonly DB_Connect _context;

        public PaimentsController(DB_Connect context)
        {
            _context = context;
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> CreatePaiments(PaimentDTO dto, int id)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Paiments data is null.");
                }

                var payment = _context.Paiments.FirstOrDefault(p => p.Id == id);
                if (payment == null)
                {
                    return NotFound($"No payment found with ID {id}");
                }

                payment.Amount = dto.total;
                payment.PaymentMethod = dto.Method;
                payment.PaymentDate = dto.Date;
                payment.UserId = dto.User; // Assuming dto.User = UserId
                payment.ReservationId = dto.Reservation; // Assuming dto.Reservation = ReservationId
                payment.TransactionId = dto.Transaction;
                payment.Currency = dto.Currency;

                if (dto.Method == "onsite" || dto.Method == "credit_card")
                {
                    payment.Status = PaymentStatus.Paid;
                }
                else
                {
                    payment.Status = PaymentStatus.Pending;
                }

                await _context.SaveChangesAsync();

                return Ok(payment);
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


        [HttpGet]
        public async Task<IActionResult> GetPaiments()
        {
            try
            {
                var paiments = await _context.Paiments.ToListAsync();
                return Ok(paiments);
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
