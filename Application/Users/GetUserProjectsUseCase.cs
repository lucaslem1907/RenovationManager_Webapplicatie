using Application.Interfaces;
using Domain.Entities;

namespace Application.Tasks
{
    public class GetUserProjectsUseCase
    {
        private readonly IProjectRepository _repo;
        public GetUserProjectsUseCase(IProjectRepository repo)
        {
            _repo = repo;

        }

        public async Task<List<Project>> Execute(Guid UserId)
        {
            var projects = await _repo.GetProjectsByUserId(UserId);
            if (projects == null) { return null; }

            return projects;

        }
    }
}
