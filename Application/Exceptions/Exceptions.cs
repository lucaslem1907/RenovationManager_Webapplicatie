using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Application.Exceptions
{

    public abstract class BaseException : Exception
    {
        public string ErrorCode { get; }
        public int StatusCode { get; }

        protected BaseException(string errorCode, string message, int statusCode = 400) : base(message)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }
    }

    public class ProjectNotFoundException : BaseException
    {
        public ProjectNotFoundException(Guid projectId) : base("PROJECT_NOT_FOUND", $"Project not found for id {projectId}", 404)
        {
        }
    }

    public class BudgetExceededException : BaseException
    {
        public BudgetExceededException(decimal? Budget) : base("BUDGET_EXCEEDED", $"Budget exceeded, total budget = {Budget}", 409)
        {
        }
    }

    public class OwnerNotFoundException : BaseException
    {
        public OwnerNotFoundException(Guid ownerId) : base("OWNER_NOT_FOUND", $"Owner not found for id {ownerId}", 404)
        {
        }
    }

    public class ExpenseNotFoundException : BaseException
    {
        public ExpenseNotFoundException(Guid expenseId) : base("EXPENSE_NOT_FOUND", $"Expense not found for id {expenseId}", 404)
        {
        }
    }

    public class RoomNotFoundException : BaseException
    {
        public RoomNotFoundException(Guid roomId) : base("ROOM_NOT_FOUND", $"Room not found for id {roomId}", 404)
        {
        }
    }

    public class NotFoundException : BaseException
    {
        public NotFoundException(string message) : base("NOT_FOUND", message, 404)
        {
        }
    }






    }

