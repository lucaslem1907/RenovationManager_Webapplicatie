using FluentResults;

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
        public BudgetExceededError(decimal? Budget) : base($"Budget exceeded, total budget = € {Budget} ")
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

    public class TaskNotFoundError : Error
    {
        public TaskNotFoundError(Guid taskId) : base($"Task not found for id {taskId}")
        {
            Metadata["ErrorCode"] = "TASK_NOT_FOUND";
        }
    }

    public class SubTaskNotFoundError : Error
    {
        public SubTaskNotFoundError(Guid subTaskId) : base($"Subtask not found for id: {subTaskId}")
        {
            Metadata["ErrorCode"] = "SUBTASK_NOT_FOUND";
        }
    }

    public class DuplicateEmailError : Error
    {
        public DuplicateEmailError(string email) : base($"A user already exists with email {email}")
        {
            Metadata["ErrorCode"] = "DUPLICATE_EMAIL";
        }
    }

    public class InvalidCredentialsError : Error
    {
        public InvalidCredentialsError() : base("Invalid login or password")
        {
            Metadata["ErrorCode"] = "INVALID_CREDENTIALS";
        }
    }

    public class UserProjectsNotFoundError : Error
    {
        public UserProjectsNotFoundError(Guid userId) : base($"No projects found for user id {userId}")
        {
            Metadata["ErrorCode"] = "USER_PROJECTS_NOT_FOUND";
        }
    }

    public class OwnerNotFoundException : BaseException
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


