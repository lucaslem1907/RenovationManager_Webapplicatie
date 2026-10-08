using Application.Interfaces;
using Application.Exceptions;

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

        public async Task<bool> Execute(Guid roomId, bool deleteExpenses)
        {
            var room = await _repo.GetRoomById(roomId);
            if (room == null)
            {
                throw new RoomNotFoundException(roomId);
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
            return true;
        }
    }
}
