using Application.Interfaces;
using Application.Exceptions;
using FluentResults;

namespace Application.Projects
{
    public class DeleteProjectUseCase
    {

        private readonly IProjectRepository _repo;

        public DeleteProjectUseCase(IProjectRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result<bool>> Execute(Guid id)
        {
            var project = await _repo.GetById(id);
            if (project == null)
            {
                return Result.Fail(new ProjectNotFoundError(id));
            }
            await _repo.Delete(project);
            await _repo.SaveChanges();
            return Result.Ok(true);
        }
    }
}
