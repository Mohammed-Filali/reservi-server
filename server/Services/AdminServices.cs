using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;
using server.Repositories;

namespace server.Services
{
    public class AdminServices : IAdminServices
    {
        private readonly DB_Connect _context;
        

        public AdminServices(DB_Connect context)
        {
            _context = context;
        }
        public async Task<int> ProfetionnalsCount()
        {
            try
            {
                var count = await (from user in _context.Users
                                   join userRole in _context.UserRoles on user.Id equals userRole.UserId
                                   join role in _context.Roles on userRole.RoleId equals role.Id
                                   where role.Name == "professionnel"
                                   select user).CountAsync();
                return count;
            }
            catch
            {
                return 0;
            }
        }
        public async Task<int> ClientsCount()
        {
            try
            {
                var count = await (from user in _context.Users
                                   join userRole in _context.UserRoles on user.Id equals userRole.UserId
                                   join role in _context.Roles on userRole.RoleId equals role.Id
                                   where role.Name == "Client"
                                   select user).CountAsync();
                return count;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> ReservationsCountThisMonth()
        {
            var now = DateOnly.FromDateTime(DateTime.Now);
            var firstDay = new DateOnly(now.Year, now.Month, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);

            return await _context.Reservations
                .Where(r => r.Date >= firstDay && r.Date <= lastDay && r.Status == ReservationStatus.Confirmed)
                .CountAsync();
        }

        public async Task<decimal> TotalRevenue()
        {
            try
            {
                return await _context.abonnementPaiments
                    .Where(p => p.Status == AbonnementPaymentStatus.Paid)
                    .SumAsync(p => p.Amount);
            }
            catch
            {
                return 0;
            }

        }


        public async Task<int> PendingPayment ()
        {
            try
            {
                return await _context.abonnementPaiments
                    .Where(p => p.Status == AbonnementPaymentStatus.Pending)
                    .CountAsync();
            }
            catch
            {
                return 0;
            }

        }
        public async Task<int> CancelingPayment()
        {
            try
            {
                return await _context.abonnementPaiments
                    .Where(p => p.Status == AbonnementPaymentStatus.Cancelled)
                    .CountAsync();
            }
            catch
            {
                return 0;
            }

        }

    }
}
