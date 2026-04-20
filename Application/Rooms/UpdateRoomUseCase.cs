using Application.Interfaces;
using Domain.Entities;
using Shared.DTO;

namespace Application.Expenses
{
    public class UpdateRoomUseCase
    {
        private readonly IRoomRepository _repo;

        public UpdateRoomUseCase(IRoomRepository repo)
        {
            _repo = repo;

        }

        public async Task<Room?> Execute(Guid roomId, RoomDto dto)
        {
            var room = await _repo.GetRoomById(roomId);
            if (room == null) { return null; }

            room.Name = dto.Name;
            room.Status = dto.Status;
            await _repo.SaveChanges(); ;
            return room;
        }
    }


}
