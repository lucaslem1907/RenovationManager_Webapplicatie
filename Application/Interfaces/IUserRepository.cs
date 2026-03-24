using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAll();
        Task<User?> GetByLogin(string login);
        Task<User?> GetById(Guid id);
        Task<bool> EmailExists(string email);
        Task Add(User user);
        Task SaveChanges();

    }
}
