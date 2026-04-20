using Application.Interfaces;
using Domain.Entities;
using Shared.DTO;

namespace Application.Users
{
    public class LoginUseCase
    {
        private IUserRepository _repo;
        private IAuthService _authService;

        public LoginUseCase(IUserRepository repo, IAuthService authService)
        {
            _repo = repo;
            _authService = authService;
        }

        public async Task<(bool succes, string? token, User? user)> Execute(UserLoginDto dto)
        {
            var user = await _repo.GetByLogin(dto.login);
            if (user == null) { return (false, null, null); }

            var valid = _authService.VerifyPassword(dto.password, user.PasswordHash);
            if (!valid) { return (false, null, null); }

            var token = _authService.GenerateToken(user);
            return (true, token, user);
        }
    }
}
