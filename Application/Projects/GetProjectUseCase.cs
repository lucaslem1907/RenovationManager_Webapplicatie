using Application.Interfaces;
using Domain.Entities;
using Application.Exceptions;
using FluentResults;

namespace Application.Projects
{
    public class GetProjectUseCase
    {
        private readonly IProjectRepository _repo;

        public GetProjectUseCase(IProjectRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result<Project>> Execute(Guid id)
        {
            var project = await _repo.GetByIdWithDetails(id);
            if (project == null)
            {
                return Result.Fail(new ProjectNotFoundError(id));
            }
            return Result.Ok(project);
        }

    }
}
