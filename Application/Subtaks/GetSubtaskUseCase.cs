using Application.Interfaces;
using Application.Exceptions;
using Domain.Entities;
using FluentResults;

namespace Application.Subtaks
{
    public class GetSubtaskUseCase
    {
        private readonly ISubtaskRepository _repo;

        public GetSubtaskUseCase(ISubtaskRepository repo)
        {
            _repo = repo;

        }

        public async Task<Result<Subtask>> GetSubTask(Guid subtaskId)
        {
            var subtask = await _repo.GetSubTask(subtaskId);
            if (subtask == null)
            {
                return Result.Fail(new SubTaskNotFoundError(subtaskId));
            }
            return Result.Ok(subtask);

        }

        public async Task<Result<IEnumerable<Subtask>>> GetAllTasks()
        {
            var subtasks = await _repo.GetAll();
            return Result.Ok(subtasks ?? Enumerable.Empty<Subtask>());
        }
    }
}

