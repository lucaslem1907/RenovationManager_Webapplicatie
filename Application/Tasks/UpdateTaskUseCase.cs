using Application.Interfaces;
using Domain.Entities;
using Shared.DTO;
using Domain.Enums;

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
            var IncompletedTasks = room.Tasks.Where(i => !i.IsCompleted);

            if (!IncompletedTasks.Any())
            {

                room.MarkCompleted();
            }

            if (IncompletedTasks.Any() && room.Status == RoomStatus.done)
            {
                room.MarkInProgress();
            }

            await _repo.SaveChanges();
            return task;
        }
    }
}
