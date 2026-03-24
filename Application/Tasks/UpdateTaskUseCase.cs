using Application.Interfaces;
using Domain.Entities;
using Shared.DTO;

namespace Application.Tasks
{
    public class UpdateTaskUseCase
    {
        private readonly ITaskRepository _repo;

        public UpdateTaskUseCase(ITaskRepository repo)
        {
            _repo = repo;

        }

        public async Task<TaskItem> Execute(Guid taskId, TaskDto dto)
        {
            var task = await _repo.GetTask(taskId);
            if (task == null) { return null; }

            task.UpdateTask(dto.Title, dto.Description, dto.IsCompleted);
            await _repo.SaveChanges();
            return task;
        }
    }
}
