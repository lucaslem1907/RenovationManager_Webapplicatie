using Application.Interfaces;
using Domain.Entities;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {

        private readonly JwtService _jwtService;
        private readonly PasswordService _passwordService;

        public AuthService(JwtService jwt, PasswordService passwordService)
        {
            _jwtService = jwt;
            _passwordService = passwordService;
        }


        public string GenerateToken(User user)
        {
            return _jwtService.GenerateToken(user);
        }

        public string HashPassword(string password)
        {
            return _passwordService.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hash)
        {
            return _passwordService.VerifyPassword(password, hash);
        }
    }
}
