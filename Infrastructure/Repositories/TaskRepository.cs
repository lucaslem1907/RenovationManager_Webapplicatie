using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    internal class TaskRepository : ITaskRepository
    {
        private DatabaseContext _db;

        public TaskRepository(DatabaseContext db)
        {
            _db = db;
        }
        public async Task Add(TaskItem task)
        {
            await _db.Tasks.AddAsync(task);
        }

        public async Task Delete(TaskItem task)
        {
            _db.Tasks.Remove(task);
        }

        public async Task<IEnumerable<TaskItem>> GetAll()
        {
            return await _db.Tasks.ToListAsync();
        }

        public async Task<TaskItem?> GetTask(Guid taskId)
        {
            return await _db.Tasks.Include(c => c.Subtasks).FirstOrDefaultAsync(d => d.Id  == taskId);
        }

        public async Task<List<TaskItem?>> GetTasksByRoomId(Guid roomId)
        {
            return await _db.Tasks.Where(x => x.RoomId == roomId).ToListAsync();
        }



        public async Task SaveChanges()
        {
            await _db.SaveChangesAsync();
        }
    }
}
