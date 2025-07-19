using server.Models;
using System.Threading.Tasks;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(ApplicationUser user);
}
