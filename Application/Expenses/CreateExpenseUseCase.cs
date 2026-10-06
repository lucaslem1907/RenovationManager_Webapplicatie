using Application.Interfaces;
using Application.Exceptions;
using Domain.Entities;
using Shared.DTO;

namespace Application.Expenses
{
    public class CreateExpenseUseCase
    {
        private readonly IExpenseRepository _repo;
        private readonly IProjectRepository _projectRepo;
        private readonly IRoomRepository _roomRepo;

        public CreateExpenseUseCase(IExpenseRepository repo, IProjectRepository projectRepo, IRoomRepository roomRepo)
        {
            _repo = repo;
            _projectRepo = projectRepo;
            _roomRepo = roomRepo;
        }

        public async Task<Expense?> Execute(Guid projectId, ExpenseDto dto)
        {
            var project = await _projectRepo.GetById(projectId);
            if (project == null)
            {
                throw new ProjectNotFoundException(projectId);
            }
            if (dto.RoomId.HasValue)
            {
                var room = await _roomRepo.GetRoomById(dto.RoomId.Value);
                if (room == null)
                {
                    throw new RoomNotFoundException(dto.RoomId.Value);
                }
            }
            var expenses = await _repo.GetExpensesByProjectId(projectId);
            var budgetExceeded = BudgetOverschreden(expenses, dto.Amount, project.Budget, dto.ForceBudget);
            if (budgetExceeded)
            {
                throw new BudgetExceededException(project.Budget);
            }


            var newExpense = new Expense(dto.Amount, dto.Name, projectId, dto.RoomId, dto.Description, dto.Status);
            await _repo.Add(newExpense);
            await _repo.SaveChanges();

            return newExpense;
        }

        private bool BudgetOverschreden(IEnumerable<Expense?> expenses, decimal amount, decimal? totalBudget, bool? force)
        {
            if (totalBudget == null) return false;
            if (force == true) return false;

            decimal currentBudget = 0;
            foreach (var item in expenses)
            {
                currentBudget += item?.Amount ?? 0;

            }
            var newBudget = currentBudget + amount;

            return newBudget > totalBudget;

        }
    }
}
