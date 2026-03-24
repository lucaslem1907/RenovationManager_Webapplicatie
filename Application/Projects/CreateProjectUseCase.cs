using Application.Interfaces;
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
            var owner = await _userRepo.GetById(dto.OwnerId);
            if (owner == null) return null;

            var project = new Project(dto.Name, owner, dto.Description);

            await _repo.Add(project);
            await _repo.SaveChanges();

            return project;
        }
    }
}
