using Application.Interfaces;
using Application.Exceptions;
using Domain.Entities;
using FluentResults;

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

        public async Task<Result<IEnumerable<TaskItem>>> GetTasksByRoomId(Guid roomId)
        {
            var room = await _roomrepo.GetRoomById(roomId);
            if (room == null)
            { 
                return Result.Fail(new RoomNotFoundError(roomId));
            }
            var tasks = await _repo.GetTasksByRoomId(roomId);
            return Result.Ok(tasks ?? Enumerable.Empty<TaskItem>());

        }

        public async Task<Result<IEnumerable<TaskItem>>> GetAllTasks()
        {
            var tasks = await _repo.GetAll();
            return Result.Ok(tasks ?? Enumerable.Empty<TaskItem>());
        }

        public async Task<Result<TaskItem>> getTask(Guid taskId)
        {
            var task = await _repo.GetTask(taskId);
            if (task == null)
            {
                return Result.Fail(new TaskNotFoundError(taskId));
            }

            return Result.Ok(task);
        }
        public async Task<Result<TaskItem>> getTaskWithSubtasks(Guid taskId)
        {
            var task = await _repo.GetTask(taskId);
            if (task == null) 
            {
                return Result.Fail(new TaskNotFoundError(taskId));
            }

            return Result.Ok(task);
        }
    }
}
