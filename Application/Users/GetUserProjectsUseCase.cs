using Application.Interfaces;
using Domain.Entities;

namespace Application.Tasks
{
    public class GetUserProjectsUseCase
    {
        private readonly IUserRepository _repo;
        public GetUserProjectsUseCase(IUserRepository repo)
        {
            _repo = repo;

        }

        public async Task<User> Execute(Guid UserId)
        {
            var user = await _repo.GetById(UserId);
            if (user == null) { return null; }

            return user;

        }
    }
}
