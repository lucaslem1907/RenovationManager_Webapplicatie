using Domain.Entities;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
        Task<string> GenerateToken(User user);
    }
}
