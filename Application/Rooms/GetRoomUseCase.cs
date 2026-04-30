using Application.Interfaces;
using Domain.Entities;

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
            if (rooms == null) { return null; }

            return rooms;

        }

        public async Task<IEnumerable<Room?>> GetAllRooms()
        {
            var rooms = await _repo.GetAll();
            if (rooms == null) { return null; }
            return rooms;
        }

        public async Task<Room?> GetRoomWithTaskAndSubTasks(Guid roomId)
        {
            var room = await _repo.GetRoomWithTaskAndSubTasks(roomId);
            if (room == null) return null;

            return room;
        }
    }
}
