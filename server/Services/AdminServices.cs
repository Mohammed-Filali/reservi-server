using Microsoft.EntityFrameworkCore;
using server.Data;
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



    }
}
