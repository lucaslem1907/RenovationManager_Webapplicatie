using Application.Interfaces;
using Application.Exceptions;
using Domain.Entities;
using Shared.DTO;
using FluentResults;

namespace Application.Projects
{
    public class UpdateProjectUseCase
    {
        private readonly IProjectRepository _repo;

        public UpdateProjectUseCase(IProjectRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result<Project>> Execute(Guid projectId, ProjectDto dto)
        {
            var project = await _repo.GetById(projectId);
            if (project == null)
            { 
                return Result.Fail(new ProjectNotFoundError(projectId)); 
            }
            project.UpdateProject(dto.Name, dto.Description, dto.Address, dto.Budget, dto.StartDate);
            await _repo.SaveChanges();
            return Result.Ok(project);

        }
    }
}
