using Application.Interfaces;
using Application.Exceptions;
using Domain.Entities;
using FluentResults;

namespace Application.Tasks
{
    public class GetUserProjectsUseCase
    {
        private readonly IProjectRepository _repo;
        public GetUserProjectsUseCase(IProjectRepository repo)
        {
            _repo = repo;

        }

        public async Task<Result<List<Project>>> Execute(Guid UserId)
        {
            var projects = await _repo.GetProjectsByUserId(UserId);
            if (projects == null)
            {
                return Result.Fail(new UserProjectsNotFoundError(UserId));
            }

            return Result.Ok(projects);

        }
    }
}
