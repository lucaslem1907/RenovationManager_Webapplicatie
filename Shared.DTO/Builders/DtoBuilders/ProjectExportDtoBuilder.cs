using Shared.DTO;

namespace Shared.Builders.DtoBuilders;

public class ProjectExportDtoBuilder
{
    private string _name = "Export Project";
    private string _address = "Street 1";
    private DateTime _startDate = DateTime.UtcNow;
    private decimal _budget = 10000;
    private decimal _spent = 0;
    private int _totalRooms;
    private int _finishedRooms;
    private List<RoomDto> _rooms = [];
    private List<ExpenseDto> _expenses = [];

    public ProjectExportDtoBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProjectExportDtoBuilder WithAddress(string address)
    {
        _address = address;
        return this;
    }

    public ProjectExportDtoBuilder WithStartDate(DateTime startDate)
    {
        _startDate = startDate;
        return this;
    }

    public ProjectExportDtoBuilder WithBudget(decimal budget)
    {
        _budget = budget;
        return this;
    }

    public ProjectExportDtoBuilder WithSpent(decimal spent)
    {
        _spent = spent;
        return this;
    }

    public ProjectExportDtoBuilder WithTotalRooms(int totalRooms)
    {
        _totalRooms = totalRooms;
        return this;
    }

    public ProjectExportDtoBuilder WithFinishedRooms(int finishedRooms)
    {
        _finishedRooms = finishedRooms;
        return this;
    }

    public ProjectExportDtoBuilder WithRooms(List<RoomDto> rooms)
    {
        _rooms = rooms;
        return this;
    }

    public ProjectExportDtoBuilder WithExpenses(List<ExpenseDto> expenses)
    {
        _expenses = expenses;
        return this;
    }

    public ProjectExportDto Build()
    {
        return new ProjectExportDto
        {
            Name = _name,
            Address = _address,
            StartDate = _startDate,
            Budget = _budget,
            Spent = _spent,
            TotalRooms = _totalRooms,
            FinishedRooms = _finishedRooms,
            Rooms = _rooms,
            Expenses = _expenses
        };
    }
}
