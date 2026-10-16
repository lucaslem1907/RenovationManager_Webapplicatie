namespace Application.Expenses
{
    public class Checks
    {
        public static bool BudgetCheck(decimal proposedAmount, decimal? totalBudget, bool? force)
        {
         return totalBudget != null && force != true && proposedAmount > totalBudget;
        }
    }
}
