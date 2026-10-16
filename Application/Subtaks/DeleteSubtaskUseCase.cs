using Application.Interfaces;
using Application.Exceptions;
using FluentResults;

namespace Application.Subtaks
{
    public class DeleteSubtaskUseCase
    {
        private readonly ISubtaskRepository _repo;
        public DeleteSubtaskUseCase(ISubtaskRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result<bool>> Execute(Guid subtaskId)
        {
            var subtask = await _repo.GetSubTask(subtaskId);
            if (subtask == null)
            {
                return Result.Fail(new SubTaskNotFoundError(subtaskId));
            }

            await _repo.Delete(subtask);
            await _repo.SaveChanges();
            return Result.Ok(true);

        }
    }
}

