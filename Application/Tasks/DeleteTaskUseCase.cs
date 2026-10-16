using Application.Interfaces;
using Application.Exceptions;
using FluentResults;

namespace Application.Tasks
{
    public class DeleteTaskUseCase
    {
        private readonly ITaskRepository _repo;
        public DeleteTaskUseCase(ITaskRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result<bool>> Execute(Guid taskId)
        {
            var task = await _repo.GetTask(taskId);
            if (task == null) { return Result.Fail(new TaskNotFoundError(taskId)); }

            await _repo.Delete(task);
            await _repo.SaveChanges();
            return Result.Ok(true);

        }
    }
}
