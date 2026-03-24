using Shared.DTO;
using Domain.Entities;


using Microsoft.AspNetCore.Http.HttpResults;
using Application.Interfaces;

namespace Application.Projects
{
    public class UpdateProjectUseCase
    {
        private readonly IProjectRepository _repo;

        public UpdateProjectUseCase(IProjectRepository repo)
        {
            _repo = repo;
        }

        public async Task<Project?> Execute(Guid projectId, ProjectDto dto)
        {
            var project = await _repo.GetById(projectId);
            if (project == null) return null;
            project.UpdateProject(dto.Name, dto.Description, dto.Address, dto.Budget, dto.StartDate);
            await _repo.SaveChanges();
            return project;
        
        }
    }
}
