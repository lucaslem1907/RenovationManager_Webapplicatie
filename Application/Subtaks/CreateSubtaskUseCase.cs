using Application.Interfaces;
using Application.Exceptions;
using Domain.Entities;
using Shared.DTO;
using FluentResults;

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

        public async Task<Result<Subtask>> Execute(Guid taskId, SubTaskDto dto)
        {
            var Task = await _taskRepository.GetTask(taskId);
            if (Task == null)
            {
                return Result.Fail(new TaskNotFoundError(taskId));
            }

            Subtask subtask = new Subtask(dto.Title, taskId, dto.IsCompleted);
            await _repo.Add(subtask);
            await _repo.SaveChanges();
            return Result.Ok(subtask);

        }
    }
}
