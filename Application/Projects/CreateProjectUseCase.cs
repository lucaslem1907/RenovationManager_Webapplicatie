using Application.Interfaces;
using Application.Exceptions;
using Domain.Entities;
using Shared.DTO;

namespace Application.Projects
{
    public class CreateProjectUseCase
    {
        private readonly IProjectRepository _repo;
        private readonly IUserRepository _userRepo;

        public CreateProjectUseCase(IProjectRepository repo, IUserRepository userRepo)
        {
            _repo = repo;
            _userRepo = userRepo;
        }

        public async Task<Project?> Execute(ProjectDto dto)
        {
            Guid ownerId = dto.OwnerId;
            var owner = await _userRepo.GetById(ownerId);

            if (owner == null)
            {
                throw new OwnerNotFoundException(ownerId);
            }
            ;

            var project = new Project(dto.Name, owner, dto.Address, dto.Description);

            await _repo.Add(project);
            await _repo.SaveChanges();

            return project;
        }
    }
}
