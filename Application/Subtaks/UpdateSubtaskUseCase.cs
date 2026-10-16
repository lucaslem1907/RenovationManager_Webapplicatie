using Application.Interfaces;
using Application.Exceptions;
using Domain.Entities;
using Shared.DTO;
using FluentResults;

namespace Application.Subtaks
{
    public class UpdateSubtaskUseCase
    {
        private readonly ISubtaskRepository _repo;
        private readonly ITaskRepository _taskRepo;

        public UpdateSubtaskUseCase(ISubtaskRepository repo, ITaskRepository taskRepo)
        {
            _repo = repo;
            _taskRepo = taskRepo;

        }

        public async Task<Result<Subtask>> Execute(Guid subtaskId, SubTaskDto dto)
        {
            var subtask = await _repo.GetSubTask(subtaskId);

            if (subtask == null) { return Result.Fail(new SubTaskNotFoundError(subtaskId)); }

            subtask.UpdateSubtask(dto.Title, dto.IsCompleted);
            var task = await _taskRepo.GetTask(subtask.TaskItemId);

            if (task == null) { return Result.Fail(new TaskNotFoundError(subtask.TaskItemId)); }
            var inCompletedTasks = task.Subtasks.Where(c => c.IsCompleted == false);

            if (inCompletedTasks.Any() == true)
            {
                if (task.IsCompleted == true)
                {
                    task.MarkAsCompleted(false);
                }
                await _repo.SaveChanges();
                return Result.Ok(subtask);
            }

            if (inCompletedTasks.Any() == false)
            {
                task.MarkAsCompleted(true);
            }
            
            await _repo.SaveChanges();
            return Result.Ok(subtask);
        }
    }
}
