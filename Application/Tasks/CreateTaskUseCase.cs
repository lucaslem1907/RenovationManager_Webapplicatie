using Application.Interfaces;
using Domain.Entities;
using Shared.DTO;
using Domain.Enums;

namespace Application.Tasks
{
    public class CreateTaskUseCase
    {
        private readonly ITaskRepository _repo;
        private readonly IRoomRepository _roomRepository;
        public CreateTaskUseCase(ITaskRepository repo, IRoomRepository roomRepo)
        {
            _repo = repo;
            _roomRepository = roomRepo;
        }

        public async Task<TaskItem> Execute(Guid roomId, TaskDto dto)
        {

            var room = await _roomRepository.GetRoomById(roomId);
            if (room == null) { return null; }


            TaskItem task = new TaskItem(dto.Title, roomId);
            if (room.Status == RoomStatus.done)
            {
                room.MarkInProgress();
            }
            await _repo.Add(task);
            await _roomRepository.SaveChanges();
            await _repo.SaveChanges();

            return task;

        }
    }
}
