using Domain.Entities;

namespace Application.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAll();
        Task<List<TaskItem?>> GetTasksByRoomId(Guid roomId);
        Task<TaskItem?> GetTask(Guid taskId);
        Task Add(TaskItem task);
        Task Delete(TaskItem task);
        Task SaveChanges();
    }
}
