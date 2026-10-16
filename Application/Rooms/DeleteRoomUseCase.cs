using Application.Interfaces;
using Application.Exceptions;
using FluentResults;

namespace Application.Rooms
{
    public class DeleteRoomUseCase
    {
        private readonly IRoomRepository _repo;
        private readonly IExpenseRepository _expenserepo;

        public DeleteRoomUseCase(IRoomRepository repo, IExpenseRepository expenseRepo)
        {
            _repo = repo;
            _expenserepo = expenseRepo;

        }

        public async Task<Result<bool>> Execute(Guid roomId, bool deleteExpenses)
        {
            var room = await _repo.GetRoomById(roomId);
            if (room == null)
            {
                return Result.Fail(new RoomNotFoundError(roomId));
            }

            var roomExpenses = (await _expenserepo.GetExpensesByRoomId(roomId))
                .ToList();

            if (deleteExpenses)
            {
                // delete all expenses for this room
                await _expenserepo.DeleteRange(roomExpenses);
            }
            else
            {
                // set roomId to null on expenses
                foreach (var expense in roomExpenses)
                {
                    expense.RoomId = null;
                }
            }
            await _repo.Delete(room);
            await _repo.SaveChanges();
            return Result.Ok(true);
        }
    }
}
