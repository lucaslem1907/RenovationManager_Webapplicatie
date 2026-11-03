using Domain.Entities;
using Shared.DTO;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAll();
        Task<User?> GetByLogin(string login);
        Task<User?> GetById(string id);
        Task<List<UserAdministrationDto>?> GetUserAdministrationBySearch(string? id, string? email, string? username, string? firstname, string? lastname);
        Task<bool> EmailExists(string email);
        Task Add(User user);
        Task SaveChanges();

    }
}
