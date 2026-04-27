using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    internal class ProjectRepository : IProjectRepository
    {
        private readonly DatabaseContext _db;

        public ProjectRepository(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<List<Project>> GetAll()
        {
            return await _db.RenovationProjects.ToListAsync();
        }

        public async Task<Project?> GetById(Guid id)
        {
            return await _db.RenovationProjects.FindAsync(id);
        }
        public async Task<List<Project>> GetProjectsByUserId(Guid userId)
        {
            return await _db.RenovationProjects.Where(p => p.OwnerId == userId).ToListAsync();
        }
        public async Task<Project?> GetByIdWithDetails(Guid id)
        {
            return await _db.RenovationProjects
                .Include(p => p.Rooms)
                    .ThenInclude(r => r.Tasks)
                .Include(p => p.Expenses)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task Add(Project project)
        {
            await _db.RenovationProjects.AddAsync(project);
        }

        public Task Delete(Project project)
        {
            _db.RenovationProjects.Remove(project);
            return Task.CompletedTask;
        }

        public async Task SaveChanges()
        {
            await _db.SaveChangesAsync();
        }
    }
}
