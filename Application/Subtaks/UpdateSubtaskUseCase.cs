using Application.Interfaces;
using Application.Exceptions;
using Domain.Entities;
using Shared.DTO;

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

        public async Task<Subtask> Execute(Guid subtaskId, SubTaskDto dto)
        {
            var subtask = await _repo.GetSubTask(subtaskId);

            if (subtask == null) { throw new SubTaskNotFoundException(subtaskId); }

            subtask.UpdateSubtask(dto.Title, dto.IsCompleted);
            var task = await _taskRepo.GetTask(subtask.TaskItemId);

            if (task == null) { throw new TaskNotFoundException(subtask.TaskItemId); }
            var inCompletedTasks = task.Subtasks.Where(c => c.IsCompleted == false);

            if (inCompletedTasks.Any() == true)
            {
                if (task.IsCompleted == true)
                {
                    task.MarkAsCompleted(false);
                }
                await _repo.SaveChanges();
                return subtask;
            }

            if (inCompletedTasks.Any() == false)
            {
                task.MarkAsCompleted(true);
            }
            
            await _repo.SaveChanges();
            return subtask;
        }
    }
}
