using Application.Interfaces;
using Domain.Entities;
using Shared.DTO;

namespace Application.Subtaks
{
    public class UpdateSubtaskUseCase
    {
        private readonly ISubtaskRepository _repo;

        public UpdateSubtaskUseCase(ISubtaskRepository repo)
        {
            _repo = repo;

        }

        public async Task<Subtask> Execute(Guid subtaskId, SubTaskDto dto)
        {
            var subtask = await _repo.GetSubTask(subtaskId);
            if (subtask == null) { return null; }

            subtask.UpdateSubtask(dto.Title, dto.IsCompleted);
            await _repo.SaveChanges();
            return subtask;
        }
    }
}
