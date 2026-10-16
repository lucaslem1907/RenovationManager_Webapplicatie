using Application.Exceptions;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Application.Exceptions
{

    public sealed class ProjectNotFoundError : Error
    {
        public ProjectNotFoundError(Guid projectId)
            : base($"Project not found for id {projectId}")
        {
            Metadata["ErrorCode"] = "PROJECT_NOT_FOUND";
        }
    }

    public class BudgetExceededError : Error
    {
        public BudgetExceededError(decimal? Budget) : base($"Budget exceeded, total budget = {Budget}")
        {
            Metadata["ErrorCode"] = "BUDGET_EXCEEDED";
        }
    }

    public class OwnerNotFoundError : Error
    {
        public OwnerNotFoundError(Guid ownerId) : base($"Owner not found for id {ownerId}")
        {
            Metadata["ErrorCode"] = "OWNER_NOT_FOUND";
        }
    }

    public class ExpenseNotFoundError : Error
    {
        public ExpenseNotFoundError(Guid expenseId) : base($"Expense not found for id {expenseId}")
        {
            Metadata["ErrorCode"] = "EXPENSE_NOT_FOUND";
        }
    }

    public class RoomNotFoundError : Error
    {
        public RoomNotFoundError(Guid roomId) : base($"Room not found for id {roomId}")
        {
            Metadata["ErrorCode"] = "ROOM_NOT_FOUND";
        }
    }

    public class NotFoundError : Error
    {
        public NotFoundError(string message) : base(message)
        {
            Metadata["ErrorCode"] = "NOT_FOUND";
        }
    }

    public class ProjectNotFoundException : Exception
    {
        public ProjectNotFoundException(Guid projectId)
            : base($"Project not found for id {projectId}")
        {
        }
    }

    public class ExpenseNotFoundException : Exception
    {
        public ExpenseNotFoundException(Guid expenseId)
            : base($"Expense not found for id {expenseId}")
        {
        }
    }

    public class RoomNotFoundException : Exception
    {
        public RoomNotFoundException(Guid roomId)
            : base($"Room not found for id {roomId}")
        {
        }
    }

    public class OwnerNotFoundException : Exception
    {
        public OwnerNotFoundException(Guid ownerId)
            : base($"Owner not found for id {ownerId}")
        {
        }
    }

    public class BaseException : Exception
    {
        public BaseException(string message) : base(message)
        {
        }
    }
}


