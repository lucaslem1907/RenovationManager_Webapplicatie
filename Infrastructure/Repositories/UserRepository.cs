using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

        public async Task SaveChanges()
        {
            await _db.SaveChangesAsync();
        }
    }
}
