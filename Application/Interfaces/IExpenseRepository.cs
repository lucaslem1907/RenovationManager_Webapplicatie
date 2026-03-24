using Domain.Entities;

namespace Application.Interfaces
{
    public interface IExpenseRepository
    {
        Task<List<Expense>> GetAll();
        Task<IEnumerable<Expense?>> GetExpensesByProjectId(Guid projectid);
        Task<Expense?> GetExpenseById(Guid id);
        Task Add(Expense expense);
        Task Delete(Expense expense);
        Task DeleteRange(IEnumerable<Expense> roomExpenses);
        Task SaveChanges();
    }
}
