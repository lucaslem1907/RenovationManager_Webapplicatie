using Application.Interfaces;
using Application.Exceptions;
using Domain.Entities;
using Shared.DTO;
using FluentResults;

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

        public async Task<Result<Expense?>> Execute(Guid projectId, ExpenseDto dto)
        {
            var project = await _projectRepo.GetById(projectId);
            if (project == null)
            {
                return Result.Fail(new ProjectNotFoundError(projectId));
            }





            if (dto.RoomId.HasValue)
            {
                var room = await _roomRepo.GetRoomById(dto.RoomId.Value);
                if (room == null || room.ProjectId != projectId)
                {
                    return Result.Fail(new RoomNotFoundError(dto.RoomId.Value));
                }
            }

            var expenses = await _repo.GetExpensesByProjectId(projectId);
            var sumExpenses = expenses.Sum(e => e.Amount) + dto.Amount;
            var budgetExceeded = Checks.BudgetCheck(sumExpenses, project.Budget, dto.ForceBudget);

            if (budgetExceeded)
            {
                return Result.Fail(new BudgetExceededError(project.Budget));
            }

            var newExpense = new Expense(dto.Amount, dto.Name, projectId, dto.RoomId, dto.Description, dto.Status);
            await _repo.Add(newExpense);
            await _repo.SaveChanges();

            return newExpense;
        }


    }
}
