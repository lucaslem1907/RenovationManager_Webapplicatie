using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Application.Interfaces;
using Application.Exceptions;
using FluentResults;
namespace Application.Administration
{
    public class AdministrationUsecase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdministrationUsecase(IUserRepository userRepository, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<Result> ExecuteAssignRole(string userId, string roleName)
        {
            var user = await _userRepository.GetById(userId);
            if (user == null)
            {
                return Result.Fail( new OwnerNotFoundError(userId));
            }

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                return Result.Fail(new RoleNotFoundError(roleName));
            }

            await _userManager.AddToRoleAsync(user, roleName);
            await _userRepository.SaveChanges();
            return Result.Ok();
        }

        public async Task<Result> ExecuteDeleteRole (string userId, string roleName)
        {
            var user = await _userRepository.GetById(userId);
            if (user == null)
            {
                return Result.Fail(new OwnerNotFoundError(userId));
            }
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                return Result.Fail(new RoleNotFoundError(roleName));
            }
            await _userManager.RemoveFromRoleAsync(user, roleName);
            await _userRepository.SaveChanges();
            return Result.Ok();
        }
    }
}
