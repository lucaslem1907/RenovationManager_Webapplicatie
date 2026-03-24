using Application.Interfaces;
using Domain.Entities;

namespace Application.Tasks
{
    public class GetTaskUseCase
    {
        private readonly ITaskRepository _repo;
        private readonly IRoomRepository _roomrepo;

        public GetTaskUseCase(ITaskRepository repo, IRoomRepository roomRepo)
        {
            _repo = repo;
            _roomrepo = roomRepo;

        }

        public async Task<IEnumerable<TaskItem>> GetTasksByRoomId(Guid roomId)
        {
            var room = await _roomrepo.GetRoomById(roomId);
            if (room == null) return null;
            var tasks = await _repo.GetTasksByRoomId(roomId);
            if (tasks == null) { return null; }

            return tasks;

        }

        public async Task<IEnumerable<TaskItem>> GetAllTasks()
        {
            var tasks = await _repo.GetAll();
            if (tasks == null) { return null; }
            return tasks;
        }

        public async Task<TaskItem> getTask(Guid taskId)
        {
            var task = await _repo.GetTask(taskId);
            if (task == null) return null;

            return task;
        }
        public async Task<TaskItem> getTaskWithSubtasks(Guid taskId)
        {
            var task = await _repo.GetTask(taskId);
            if (task == null) return null;

            return task;
        }
    }
}
