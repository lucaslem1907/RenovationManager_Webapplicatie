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
            await _db.Users.AddAsync(user);
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _db.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<List<User>> GetAll()
        {
            return await _db.Users.Include(u => u.Projects).ToListAsync();
        }

        public async Task<User?> GetById(Guid id)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByLogin(string login)
        {
            return await _db.Users.Include(u => u.Projects).FirstOrDefaultAsync(u => u.Login == login);
        }

        public async Task SaveChanges()
        {
            await _db.SaveChangesAsync();
        }
    }
}
