using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private DatabaseContext _db;

        public UserRepository(DatabaseContext db)
        {
            _db = db;
        }

        public async Task Add(User user)
        {
            await _db.Set<User>().AddAsync(user);
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _db.Set<User>().AnyAsync(u => u.Email == email);
        }

        public async Task<List<User>> GetAll()
        {
            return await _db.Set<User>().Include(u => u.Projects).ToListAsync();
        }

        public async Task<User?> GetById(string id)
        {
            return await _db.Set<User>().FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByLogin(string login)
        {
            var normalizedLogin = login.ToUpperInvariant();

            return await _db.Set<User>()
                .Include(u => u.Projects)
                .FirstOrDefaultAsync(u =>
                    u.UserName == login ||
                    u.Login == login ||
                    u.NormalizedUserName == normalizedLogin);
        }

        public async Task<List<UserAdministrationDto>?> GetUserAdministrationBySearch(string? id, string? email, string? username, string? firstname, string? lastname)
        {
            var query = _db.Set<User>().AsQueryable();
            if (!string.IsNullOrEmpty(id))
            {
                query = query.Where(u => u.Id == id);
            }
            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(u => u.Email.Contains(email));
            }
            if (!string.IsNullOrEmpty(username))
            {
                query = query.Where(u => u.UserName.Contains(username));
            }
            if (!string.IsNullOrEmpty(firstname))
            {
                query = query.Where(u => u.FirstName.Contains(firstname));
            }
            if (!string.IsNullOrEmpty(lastname))
            {
                query = query.Where(u => u.LastName.Contains(lastname));
            }
            // Project the results to UserAdministrationDto
            return await query
                .Select(u => new UserAdministrationDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Roles = _db.Set<IdentityUserRole<string>>()
                        .Where(ur => ur.UserId == u.Id)
                        .Join(_db.Set<IdentityRole>(),
                            ur => ur.RoleId,
                            r => r.Id,
                            (ur, r) => r.Name)
                        .Select(name => name!)
                        .ToList()
                })
                .ToListAsync();

        }

        public async Task SaveChanges()
        {
            await _db.SaveChangesAsync();
        }
    }
}
