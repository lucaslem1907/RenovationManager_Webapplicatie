using Shared.DTO;

namespace Shared.Builders.DtoBuilders;

public class ExpenseDtoBuilder
{
    private string _name = "Material";
    private string _description = "Default expense";
    private ExpenseStatus _status = ExpenseStatus.unpaid;
    private Guid? _roomId;
    private decimal _amount = 100;
    private DateTime _createdDate = DateTime.UtcNow;
    private bool _forceBudget;

    public ExpenseDtoBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ExpenseDtoBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public ExpenseDtoBuilder WithStatus(ExpenseStatus status)
    {
        _status = status;
        return this;
    }

    public ExpenseDtoBuilder WithRoomId(Guid? roomId)
    {
        _roomId = roomId;
        return this;
    }

    public ExpenseDtoBuilder WithAmount(decimal amount)
    {
        _amount = amount;
        return this;
    }

    public ExpenseDtoBuilder WithCreatedDate(DateTime createdDate)
    {
        _createdDate = createdDate;
        return this;
    }

    public ExpenseDtoBuilder WithForceBudget(bool forceBudget)
    {
        _forceBudget = forceBudget;
        return this;
    }

    public ExpenseDto Build()
    {
        return new ExpenseDto
        {
            Name = _name,
            Description = _description,
            Status = _status,
            RoomId = _roomId,
            Amount = _amount,
            CreatedDate = _createdDate,
            ForceBudget = _forceBudget
        };
    }
}
