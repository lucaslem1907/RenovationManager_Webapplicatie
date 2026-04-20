using Application.Interfaces;
using Domain.Entities;
using Shared.DTO;

namespace Application.Expenses
{
    public class CreateRoomUseCase
    {
        private readonly IRoomRepository _repo;
        private readonly IProjectRepository _projectrepo;

        public CreateRoomUseCase(IRoomRepository repo, IProjectRepository projectrep)
        {
            _repo = repo;
            _projectrepo = projectrep;
        }

        public async Task<Room?> Execute(Guid projectId, RoomDto dto)
        {
            var project = await _projectrepo.GetById(projectId);
            if (project == null) { return null; }

            var newRoom = new Room
            {
                Name = dto.Name,
                Status = dto.Status,
                ProjectId = projectId
            };

            await _repo.Add(newRoom);
            await _repo.SaveChanges();

            return newRoom;
        }
    }
}
