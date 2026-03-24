using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    internal class RoomRepository : IRoomRepository
    {
        private DatabaseContext _db;

        public RoomRepository(DatabaseContext db)
        {
            _db = db;
        }

        public async Task Add(Room room)
        {
            await _db.Rooms.AddAsync(room);
        }

        public async Task Delete(Room room)
        {
            _db.Rooms.Remove(room);
        }

        public async Task<List<Room>> GetAll()
        {
            return await _db.Rooms.ToListAsync();
        }

        public async Task<Room?> GetRoomById(Guid id)
        {
            return await _db.Rooms.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Room?>> GetRoomsByProjectId(Guid projectId)
        {
            return await _db.Rooms.Where(x => x.ProjectId == projectId).ToListAsync();
        }

        public async Task<List<Room>> GetRoomWithTaskAndSubTasks(Guid roomId)
        {
            return await _db.Rooms.Include(x => x.Tasks)
                .ThenInclude(x=> x.Subtasks).Where(x => x.Id == roomId)
                .ToListAsync(); 
        }

        public async Task SaveChanges()
        {
            await _db.SaveChangesAsync();
        }
    }
}
