using Application.Interfaces;
using Domain.Entities;

namespace Application.Subtaks
{
    public class GetSubtaskUseCase
    {
        private readonly ISubtaskRepository _repo;

        public GetSubtaskUseCase(ISubtaskRepository repo)
        {
            _repo = repo;

        }

        public async Task<Subtask> GetSubTask(Guid subtaskId)
        {
            var subtask = await _repo.GetSubTask(subtaskId);
            if (subtask == null) { return null; }
            return subtask;

        }

        public async Task<IEnumerable<Subtask>> GetAllTasks()
        {
            var subtasks = await _repo.GetAll();
            if (subtasks == null) { return null; }
            return subtasks;
        }

        public async Task<Subtask> getSubTask(Guid subtaskId)
        {
            var subtask = await _repo.GetSubTask(subtaskId);
            if (subtask == null) return null;

            return subtask;
        }
    }
}

