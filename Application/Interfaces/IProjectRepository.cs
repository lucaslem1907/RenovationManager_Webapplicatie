using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAll();
        Task<List<Project>> GetProjectsByUserId(Guid userId);
        Task<Project?> GetById(Guid id);
        Task<Project?> GetByIdWithDetails(Guid id);
        Task Add(Project project);
        Task Delete(Project project);
        Task SaveChanges();
    }
}
