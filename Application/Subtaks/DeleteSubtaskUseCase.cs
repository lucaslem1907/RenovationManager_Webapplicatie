using Application.Interfaces;
using Application.Exceptions;

namespace Application.Subtaks
{
    public class DeleteSubtaskUseCase
    {
        private readonly ISubtaskRepository _repo;
        public DeleteSubtaskUseCase(ISubtaskRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Execute(Guid subtaskId)
        {
            var subtask = await _repo.GetSubTask(subtaskId);
            if (subtask == null)
            {
                throw new SubTaskNotFoundException(subtaskId);
            }

            await _repo.Delete(subtask);
            await _repo.SaveChanges();
            return true;

        }
    }
}

