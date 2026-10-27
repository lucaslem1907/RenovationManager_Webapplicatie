using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAll();
        Task<User?> GetByLogin(string login);
        Task<User?> GetById(string id);
        Task<bool> EmailExists(string email);
        Task Add(User user);
        Task SaveChanges();

    }
}
