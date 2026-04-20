namespace Shared.DTO
{
    public class ExpenseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public ExpenseStatus Status { get; set; }

        public Guid? RoomId { get; set; }

        public decimal Amount { get; set; }

        public bool ForceBudget { get; set; }


    }
}
