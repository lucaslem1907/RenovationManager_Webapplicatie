using Application.Interfaces;
using Domain.Entities;
using Application.Exceptions;

namespace Application.Projects
{
    public class GetProjectUseCase
    {
        private readonly IProjectRepository _repo;

        public GetProjectUseCase(IProjectRepository repo)
        {
            _repo = repo;
        }

        public async Task<Project?> Execute(Guid id)
        {
            var project = await _repo.GetByIdWithDetails(id);
            if (project == null)
            {
                throw new ProjectNotFoundException(id);
            }
            return project;
        }

    }
}
