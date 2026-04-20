using Domain.Entities;

namespace Application.Interfaces
{
    public interface ISubtaskRepository
    {
        Task<IEnumerable<Subtask>> GetAll();
        Task<Subtask?> GetSubTask(Guid taskId);
        Task Add(Subtask subtask);
        Task Delete(Subtask subtask);
        Task SaveChanges();
    }
}
