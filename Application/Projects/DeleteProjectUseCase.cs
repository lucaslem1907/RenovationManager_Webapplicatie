using Application.Interfaces;

namespace Application.Projects
{
    public class DeleteProjectUseCase
    {

        private readonly IProjectRepository _repo;

        public DeleteProjectUseCase(IProjectRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Execute(Guid id)
        {
            var project = await _repo.GetById(id);
            if (project == null) return false;

            await _repo.Delete(project);
            await _repo.SaveChanges();
            return true;
        }
    }
}
