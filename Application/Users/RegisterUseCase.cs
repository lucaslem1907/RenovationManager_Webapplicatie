using Application.Interfaces;
using Domain.Entities;
using Shared.DTO;

namespace Application.Users
{
    public class RegisterUseCase
    {
        private IUserRepository _repo;
        private IAuthService _authService;

        public RegisterUseCase(IUserRepository repo, IAuthService authService)
        {
            _repo = repo;
            _authService = authService;
        }

        public async Task<bool> Execute(UserRegisterDto dto)
        {
            if (await _repo.EmailExists(dto.Email)) { return false; }

            var hash = _authService.HashPassword(dto.Password);
            var user = new User(dto.FirstName,
                                dto.LastName,
                                dto.Email,
                                hash);

            await _repo.Add(user);
            await _repo.SaveChanges();
            return true;
            
        }



    }
}
