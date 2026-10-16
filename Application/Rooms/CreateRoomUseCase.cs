using Application.Interfaces;
using Application.Exceptions;
using Domain.Entities;
using Shared.DTO;
using FluentResults;

namespace Application.Rooms
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

        public async Task<Result<Room>> Execute(Guid projectId, RoomDto dto)
        {
            var project = await _projectrepo.GetById(projectId);
            if (project == null)
            {
                return Result.Fail(new ProjectNotFoundError(projectId));
            }

            var newRoom = new Room
            {
                Name = dto.Name,
                Status = dto.Status,
                Note = dto.Note,
                ProjectId = projectId
            };

            await _repo.Add(newRoom);
            await _repo.SaveChanges();

            return Result.Ok(newRoom);
        }
    }
}
