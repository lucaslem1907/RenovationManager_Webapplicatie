using Application.Interfaces;
using Application.Exceptions;
using Domain.Entities;
using Shared.DTO;

namespace Application.Rooms
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
            if (room == null)
            { 
                throw new RoomNotFoundException(roomId);
            }

            room.Name = dto.Name;
            room.Status = dto.Status;
            room.Note = dto.Note;
            await _repo.SaveChanges(); ;
            return room;
        }
    }


}
