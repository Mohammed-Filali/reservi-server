using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Repositories;

namespace server.Services
{
    public class PaimentService : IPaimentService
    {
        private readonly DB_Connect _context;

        public PaimentService( DB_Connect context)
        {
            _context = context;
        }

        public async Task<bool> defaultPaiment(string userId, int reservationID)
        {
            try
            {
                var reservation = await _context.Reservations
                    .Where(r => r.Id == reservationID)
                    .Include(s => s.Service)
                    .Select(s => s.Service.Price)
                    .FirstOrDefaultAsync();

                var paiment = new Models.Paiment
                {
                    UserId = userId,
                    ReservationId = reservationID,
                    Amount = reservation, 
                    PaymentMethod = "",
                    PaymentDate = DateTime.Now,
                    
                };
                _context.Paiments.Add(paiment);
                await _context.SaveChangesAsync();
                return true;

            } catch (Exception ex)
            {
                // Log the exception (ex) as needed
                return false;
            }
        }
    }
}
