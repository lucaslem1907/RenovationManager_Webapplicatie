using Application.Interfaces;
using Domain.Entities;
using Shared.DTO;

namespace Application.Tasks
{
    public class UpdateTaskUseCase
    {
        private readonly ITaskRepository _repo;
        private readonly IRoomRepository _roomRepo;

        public UpdateTaskUseCase(ITaskRepository repo, IRoomRepository roomRepo)
        {
            _repo = repo;
            _roomRepo = roomRepo;

        }

        public async Task<TaskItem> Execute(Guid taskId, TaskDto dto)
        {
            var task = await _repo.GetTask(taskId);
            if (task == null) { return null; }

            task.UpdateTask(dto.Title, dto.Description, dto.IsCompleted);

            var room = await _roomRepo.GetRoomWithTaskAndSubTasks(task.RoomId);
            var inCompletedTasks = room.Tasks.Where(i => i.IsCompleted);

            if (inCompletedTasks.Any())
            {

                room.MarkCompleted();
            }

            await _repo.SaveChanges();
            return task;
        }
    }
}
