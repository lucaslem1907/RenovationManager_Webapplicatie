using Application.Interfaces;
using Domain.Entities;
using Shared.DTO;

namespace Application.Tasks
{
    public class CreateSubTaskUseCase
    {
        private readonly ISubtaskRepository _repo;
        private readonly ITaskRepository _taskRepository;
        public CreateSubTaskUseCase(ISubtaskRepository repo, ITaskRepository taskRepo)
        {
            _repo = repo;
            _taskRepository = taskRepo;
        }

        public async Task<Subtask> Execute(Guid taskId, SubTaskDto dto)
        {
            var Task = await _taskRepository.GetTask(taskId);
            if (Task == null) { return null; }

            Subtask subtask = new Subtask(dto.Title, taskId, dto.IsCompleted);
            await _repo.Add(subtask);
            await _repo.SaveChanges();
            return subtask;

        }
    }
}
