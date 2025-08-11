using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOS;
using server.Models;
using server.Repositories;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly DB_Connect _context;
        private readonly IPaimentService _paimentService;
        public ReservationsController( DB_Connect context  , IPaimentService paimentService)
        {
            _context = context;
            _paimentService = paimentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation(ReservationDTOS dto)
        {
            if (dto == null)
            {
                return BadRequest("Reservation data is null.");
            }

            try
            {
               var validationResults = _context.Reservations
                    .Where(r => r.Date == dto.Date && r.StartTime == dto.Time && r.ServiceId == dto.Service)
                    ;
                if (validationResults.Any())
                {
                    return BadRequest("A reservation already exists for this date, time, and service.");
                }
                var reservation = new Reservation
                {
                    Date = dto.Date,
                    StartTime = dto.Time,
                    UserId = dto.User,
                    ServiceId = dto.Service,
                    Status = ReservationStatus.Pending // Default status can be set to Pending
                };

                await _context.Reservations.AddAsync(reservation);
                // Automatically create a payment record for the reservation
                await _context.SaveChangesAsync();

                var paymentCreated = await _paimentService.defaultPaiment(dto.User, reservation.Id);

               
                return Ok(new
                {
                    reservation.Id,
                    reservation.Date,
                    reservation.StartTime,
                    reservation.Status,
                    reservation.UserId,
                    reservation.ServiceId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating reservation: {ex.Message}");
            }
        }


        [HttpGet("client/{id}")]
        public async Task<IActionResult> GetReservationsByClient(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Client ID is required.");
            }
            try
            {
                var reservations = await _context.Reservations
                    .Where(r => r.UserId == id)
                    .Include(r => r.Service)
                    .Select(reservations => new
                    {
                        reservations.Id,
                        reservations.Date,
                        reservations.StartTime,
                        status = reservations.Status.ToString(),
                        reservations.Service.Professional.BusinessName, // Assuming Service has a Name property
                        reservations.Service.Professional.Address,
                        reservations.Service.Title,
                        reservations.Service.Duration,
                        reservations.Service.Price ,
                        paymenStatus = reservations.Paiment.Status.ToString() 
                    }).ToListAsync();
                if (reservations == null || !reservations.Any())
                {
                    return NotFound("No reservations found for this client.");
                }
                return Ok( reservations );
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving reservations: {ex.Message}");
            }
        }
        [HttpGet("Pro/{id}")]
        public async Task<IActionResult> GetReservationsByPro(int id)
        {
            if (id == null)
            {
                return BadRequest("Client ID is required.");
            }
            try
            {
                var reservations = await _context.Reservations
                    .Where(r => r.Service.ProfessionalId == id)
                    .Include(r => r.Service)
                    .Include(r => r.User) // Include User to get user details   
                    .Select(reservations => new
                    {
                        reservations.Id,
                        reservations.Date,
                        reservations.StartTime,
                       status = reservations.Status.ToString(),
                        reservations.User.UserName , // Assuming Service has a Name property
                        reservations.Service.Title,
                        reservations.Service.Duration,
                        reservations.Service.Price,
                        paymenStatus = reservations.Paiment.Status.ToString()

                    })
                    .ToListAsync();
                if (reservations == null || !reservations.Any())
                {
                    return NotFound("No reservations found for this client.");
                }
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving reservations: {ex.Message}");
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservationById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid reservation ID.");
            }
            try
            {
                var reservation = await _context.Reservations
                    .Where(r => r.Id == id)
                    .Include(r => r.Service)
                    .Include(r => r.User) // Include User to get user details
                    .Select(reservation => new
                    {
                        reservation.Id,
                        reservation.Date,
                        reservation.StartTime,
                        status = reservation.Status.ToString(),
                        reservation.Service.Professional.BusinessName, // Assuming Service has a Name property
                        reservation.Service.Professional.Address,
                        reservation.Service.Title,
                        reservation.Service.Duration,
                        reservation.Service.Price,
                        reservation.UserId,
                        paymenStatus = reservation.Paiment.Status.ToString(),
                        paymentID=reservation.Paiment.Id


                    })
                    .FirstOrDefaultAsync();
                if (reservation == null)
                {
                    return NotFound("Reservation not found.");
                }
                return Ok(reservation);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving reservation: {ex.Message}");
            }
        }

        [HttpPatch("{status}/{id}")]
        public async Task<IActionResult> updateStatus(string status,int id)
        {
            try { 
                var reservation = await _context.Reservations.FindAsync(id);
                if (reservation == null)
                {
                    return NotFound("Reservation not found.");
                }
                if(status == "Confirmed")
                {
                    reservation.Status = ReservationStatus.Confirmed;
                }
                else if (status == "Cancelled")
                {
                    reservation.Status = ReservationStatus.Cancelled;
                }
                else
                {
                    return BadRequest("Invalid status value.");
                }
                await _context.SaveChangesAsync();
                return Ok(new { reservation.Id, status = reservation.Status.ToString() });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating reservation status: {ex.Message}");
            }

        }

        [HttpGet("Admin")]
        public async Task<IActionResult> GetReservations()
        {
            try
            {
                var reservations = await _context.Reservations
                    .Include(r => r.Service)
                    .Include(r => r.User)
                    .Select(reservation => new
                    {
                        reservation.Id,
                        reservation.Date,
                        reservation.StartTime,
                        status = reservation.Status.ToString(),
                        reservation.Service.Professional.BusinessName,
                        reservation.Service.Professional.Address,
                        reservation.Service.Title,
                        reservation.Service.Duration,
                        reservation.Service.Price,
                        reservation.UserId,
                        reservation.User.UserName,
                        paymenStatus = reservation.Paiment != null ? reservation.Paiment.Status.ToString() : null,
                        paymentID = reservation.Paiment != null ? reservation.Paiment.Id : (int?)null
                    })
                    .ToListAsync();

                return Ok(reservations);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving reservation: {ex.Message}");
            }
        }
    }

}