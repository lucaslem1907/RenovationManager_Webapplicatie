using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    internal class SubtaskRepository : ISubtaskRepository
    {

        private DatabaseContext _db;

        public SubtaskRepository(DatabaseContext db) { _db =db; }
        public async Task Add(Subtask subtask)
        {
            await _db.Subtasks.AddAsync(subtask);
        }
        public async Task Delete(Subtask subtask)
        {
            _db.Subtasks.Remove(subtask);
        }

        public async Task<IEnumerable<Subtask>> GetAll()
        {
            return await _db.Subtasks.ToListAsync();
        }

        public async Task<Subtask?> GetSubTask(Guid taskId)
        {
            return await _db.Subtasks.Where(x => x.Id  == taskId).FirstOrDefaultAsync();
        }

        public async Task SaveChanges()
        {
            await _db.SaveChangesAsync();
        }
    }
}
