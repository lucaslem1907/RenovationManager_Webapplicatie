using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Shared.DTO;

namespace Application.Users
{
    public class RegisterUseCase
    {
        private IUserRepository _repo;
        private IAuthService _authService;
        private UserManager<User> _userManager;

        public RegisterUseCase(IUserRepository repo, IAuthService authService, UserManager<User> userManager)
        {
            _repo = repo;
            _authService = authService;
            _userManager = userManager;
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
            await _userManager.AddToRoleAsync(user, "User");

            return true;

        }



    }
}
