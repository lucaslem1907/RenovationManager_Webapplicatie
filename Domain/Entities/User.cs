namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }

        public string Login { get; private set; }
        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime Created { get; private set; }

        public ICollection<Project> Projects { get; private set; } = new List<Project>();

        private User() { }

        public User(string firstName, string lastName, string email, string passwordHash)
        {
            Id = Guid.NewGuid();
            LoginGenerator loginObj = new LoginGenerator(firstName, lastName);
            Login = loginObj.Value;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Created = DateTime.UtcNow;
            PasswordHash = passwordHash;
        }
    }

    public class LoginGenerator
    {
        public string Value { get; private set; }

        public LoginGenerator(string firstname, string lastName)
        {
            Value = LoginGenerator.CreateLogin(firstname, lastName);
        }

        private LoginGenerator() { }

        public static string CreateLogin(string firstname, string lastName)
        {
            var firstPart = firstname[0].ToString().ToLower();
            var secondPart = lastName.ToLower();

            return $"{firstPart}{secondPart}";
        }

    }
}
