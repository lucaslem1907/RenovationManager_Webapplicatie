namespace Application.Exceptions
{
    public class ProjectNotFoundException : Exception
    {
        public ProjectNotFoundException(string message) : base(message)
        {
        }
    }

    public class BudgetExceededException : Exception
    {
        public BudgetExceededException(string message) : base(message)
        {
        }
    }


}


