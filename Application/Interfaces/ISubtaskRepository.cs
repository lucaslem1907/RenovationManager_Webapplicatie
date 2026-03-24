using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISubtaskRepository
    {
        Task<IEnumerable<Subtask>> GetAll();
        Task<Subtask?> GetSubTask(Guid taskId);
        Task Add(Subtask subtask);
        Task Delete(Subtask subtask);
        Task SaveChanges();
    }
}
