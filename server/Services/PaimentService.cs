using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;
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

        public async Task<bool> isAbonnementPayed(int proId)
        {
            try
            {
                var now = DateTime.Now;

                var abonnement = await _context.abonnementPaiments
                    .Include(a => a.Abonnement)
                    .Where(a => a.ProfetionnalId == proId)
                    .OrderByDescending(a => a.PaymentDate)
                    .FirstOrDefaultAsync();

              

                // Calcul de la date d'expiration
                DateTime expirationDate = abonnement.PaymentDate;
                switch (abonnement.Abonnement.DurationUnit?.ToLower())
                {
                    case "day":
                        expirationDate = expirationDate.AddDays(abonnement.Abonnement.DurationValue);
                        break;
                    case "month":
                        expirationDate = expirationDate.AddMonths(abonnement.Abonnement.DurationValue);
                        break;
                    case "year":
                        expirationDate = expirationDate.AddYears(abonnement.Abonnement.DurationValue);
                        break;
                }

                // Vérifie le statut et la validité
                if ((abonnement.Status == AbonnementPaymentStatus.Paid || abonnement.Status == AbonnementPaymentStatus.Pending)
                    && expirationDate > now)
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
