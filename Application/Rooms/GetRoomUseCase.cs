using Application.Interfaces;
using Application.Exceptions;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Application.Rooms
{
    public class GetRoomUseCase
    {
        private readonly IRoomRepository _repo;

        public GetRoomUseCase(IRoomRepository repo)
        {
            _repo = repo;

        }

        public async Task<IEnumerable<Room?>> GetRoomsByProjectId(Guid projectId)
        {
            var rooms = await _repo.GetRoomsByProjectId(projectId);
            if (rooms == null)
            { 
                throw new ProjectNotFoundException(projectId); 
            }
            return rooms;

        }

        public async Task<IEnumerable<Room?>> GetAllRooms()
        {
            var rooms = await _repo.GetAll();
            if (rooms == null)
            {
                throw new NotFoundException("No rooms were found");
            }
            return rooms;
        }

        public async Task<Room?> GetRoomWithTaskAndSubTasks(Guid roomId)
        {
            var room = await _repo.GetRoomWithTaskAndSubTasks(roomId);
            if (room == null)
            {
                throw new RoomNotFoundException(roomId);
            }
            return room;
        }
    }
}
