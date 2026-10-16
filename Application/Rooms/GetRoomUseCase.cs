using Application.Interfaces;
using Application.Exceptions;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using FluentResults;

namespace Application.Rooms
{
    public class GetRoomUseCase
    {
        private readonly IRoomRepository _repo;

        public GetRoomUseCase(IRoomRepository repo)
        {
            _repo = repo;

        }

        public async Task<Result<IEnumerable<Room?>>> GetRoomsByProjectId(Guid projectId)
        {
            var rooms = await _repo.GetRoomsByProjectId(projectId);
            if (rooms == null)
            { 
                return Result.Fail(new ProjectNotFoundError(projectId));
            }
            return rooms;

        }

        public async Task<Result<IEnumerable<Room?>>> GetAllRooms()
        {
            var rooms = await _repo.GetAll();
            if (rooms == null)
            {
                return Result.Fail(new RoomNotFoundError(Guid.Empty));
            }
            return rooms;
        }

        public async Task<Result<Room>> GetRoomWithTaskAndSubTasks(Guid roomId)
        {
            var room = await _repo.GetRoomWithTaskAndSubTasks(roomId);
            if (room == null)
            {
                return Result.Fail(new RoomNotFoundError(roomId));
            }
            return room;
        }
    }
}
