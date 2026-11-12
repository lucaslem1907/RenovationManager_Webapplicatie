using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Application.Interfaces;
using Application.Exceptions;
using FluentResults;
using Shared.DTO;
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

            var UserRole = await _userManager.GetRolesAsync(user);
            if (UserRole.Contains(roleName))
            {
                return Result.Fail(new RoleAlreadyAssignedError(roleName));
            }
            if (UserRole.Count > 0)
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, UserRole);

                if (!removeResult.Succeeded)
                {
                    return Result.Fail(new RoleDeletingError(roleName));
                }
            }
            await _userManager.AddToRoleAsync(user, roleName);
            await _userRepository.SaveChanges();
            return Result.Ok();
        }

        public async Task<Result> ExecuteDeleteRole(string userId, string roleName)
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

            if (await _userManager.IsInRoleAsync(user, roleName))
            {
                var removeResult = await _userManager.RemoveFromRoleAsync(user, roleName);
                if (!removeResult.Succeeded)
                {
                    return Result.Fail(new RoleDeletingError(roleName));
                }
            }
            else
            {
                return Result.Fail(new RoleNotAssignedError(roleName));
            }
            await _userRepository.SaveChanges();
            return Result.Ok();
        }

        public async Task<Result<List<UserAdministrationDto>?>> ExecuteGetFullUserDetail(string? id, string? email, string? username, string? firstname, string? lastname)
        {
            var users = await _userRepository.GetUserAdministrationBySearch(id, email, username, firstname, lastname);
            
            if (users == null || !users.Any())
            {
                return Result.Fail(new OwnerNotFoundError(id));
            }
            return Result.Ok(users);
        }  

    }
}

