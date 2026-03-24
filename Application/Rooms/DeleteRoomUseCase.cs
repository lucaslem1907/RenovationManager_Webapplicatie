using Application.Interfaces;

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
            if (room == null || room.Equals("[]")) { return false; }

            if (deleteExpenses)
            {
                // delete all expenses for this room
                await _expenserepo.DeleteRange(room.Expenses);
            }
            else
            {
                // set roomId to null on expenses
                foreach (var expense in room.Expenses)
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
