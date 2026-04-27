using Application.Interfaces;
using Domain.Entities;

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
            return await _repo.GetByIdWithDetails(id);
        }

    }
}
