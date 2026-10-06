namespace Application.Exceptions
{

    public abstract class BaseException : Exception
    {
        public string ErrorCode { get; }
        protected BaseException(string errorCode,  string message) : base(message)
        {
            ErrorCode = errorCode;
        }

        protected BaseException(string errorCode, string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
    public class ProjectNotFoundException : BaseException
    {
        public ProjectNotFoundException(Guid projectId) : base("PROJECT_NOT_FOUND", $"Project not found for id {projectId}")
        {
        }
    }

    public class BudgetExceededException : BaseException
    {
        public BudgetExceededException(decimal? Budget) : base("BUDGET_EXCEEDED", $"Budget exceeded: {Budget}")
        {
        }
    }

    public class OwnerNotFoundException : BaseException 
    {
        public OwnerNotFoundException(Guid ownerId) : base("OWNER_NOT_FOUND", $"Owner not found for id {ownerId}")
        {
        }
    }

    public class ExpenseNotFoundException : BaseException
    {
        public ExpenseNotFoundException(Guid expenseId) : base("EXPENSE_NOT_FOUND", $"Expense not found for id {expenseId}")
        {
        }
    }

    public class RoomNotFoundException : BaseException
    {
        public RoomNotFoundException(Guid roomId) : base("ROOM_NOT_FOUND", $"Room not found for id {roomId}")
        {
        }
    }

    public class NotFoundException : BaseException
    {
        public NotFoundException(string message) : base("NOT_FOUND", message)
        {
        }
    }

}


