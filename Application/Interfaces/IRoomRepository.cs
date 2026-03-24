using Domain.Entities;

namespace Application.Interfaces
{
    public interface IRoomRepository
    {
        Task<List<Room>> GetAll();
        Task<List<Room?>> GetRoomsByProjectId(Guid projectId);
        Task<List<Room?>> GetRoomWithTaskAndSubTasks(Guid roomId);
        Task<Room?> GetRoomById(Guid id);

        Task Add(Room room);
        Task Delete(Room room);
        Task SaveChanges();
    }
}
