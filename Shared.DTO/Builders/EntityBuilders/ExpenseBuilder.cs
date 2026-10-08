using Domain.Entities;

namespace Shared.Builders.EntityBuilders;

public class ExpenseBuilder
{
    private decimal _amount = 100;
    private string _name = "Material";
    private Guid _projectId = Guid.NewGuid();
    private Guid? _roomId;
    private string _description = "Default expense";
    private ExpenseStatus _status = ExpenseStatus.unpaid;

    public ExpenseBuilder WithAmount(decimal amount)
    {
        _amount = amount;
        return this;
    }

    public ExpenseBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ExpenseBuilder WithProjectId(Guid projectId)
    {
        _projectId = projectId;
        return this;
    }

    public ExpenseBuilder WithRoomId(Guid? roomId)
    {
        _roomId = roomId;
        return this;
    }

    public ExpenseBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public ExpenseBuilder WithStatus(ExpenseStatus status)
    {
        _status = status;
        return this;
    }

    public Expense Build()
    {
        return new Expense(_amount, _name, _projectId, _roomId, _description, _status);
    }
}
