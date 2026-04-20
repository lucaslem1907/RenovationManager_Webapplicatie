using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    internal class ExpenseRepository : IExpenseRepository
    {
        DatabaseContext _db { get; set; }
        public ExpenseRepository(DatabaseContext db) { _db = db; }
        public async Task Add(Expense expense)
        {
            await _db.Expenses.AddAsync(expense);
        }

        public Task Delete(Expense expense)
        {
            _db.Expenses.Remove(expense);
            return Task.CompletedTask;
        }

        public Task DeleteRange(IEnumerable<Expense> roomExpenses)
        {
            _db.Expenses.RemoveRange(roomExpenses);
            return Task.CompletedTask;
        }

        public async Task<List<Expense>> GetAll()
        {
            return await _db.Expenses.ToListAsync();
        }

        public async Task<Expense?> GetExpenseById(Guid id)
        {
            return await _db.Expenses.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Expense?>> GetExpensesByProjectId(Guid projectid)
        {
            return await _db.Expenses
                .Where(d => d.ProjectId == projectid).ToListAsync();
        }

        public async Task SaveChanges()
        {
            await _db.SaveChangesAsync();
        }
    }
}
