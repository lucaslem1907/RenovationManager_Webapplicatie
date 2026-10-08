using Application.Interfaces;
using Application.Exceptions;

namespace Application.Tasks
{
    public class DeleteTaskUseCase
    {
        private readonly ITaskRepository _repo;
        public DeleteTaskUseCase(ITaskRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Execute(Guid taskId)
        {
            var task = await _repo.GetTask(taskId);
            if (task == null) { throw new TaskNotFoundException(taskId); }

            await _repo.Delete(task);
            await _repo.SaveChanges();
            return true;

        }
    }
}
