using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser
    {
        public string Login { get; private set; }
        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public DateTime Created { get; private set; }

        public ICollection<Project> Projects { get; private set; } = new List<Project>();

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User(string firstName, string lastName, string email, string passwordHash)
        {
            Id = Guid.NewGuid().ToString();
            string loginObj = GenerateLogin(firstName, lastName);
            Login = loginObj;
            UserName = Login;
            NormalizedUserName = UserName.ToUpperInvariant();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            NormalizedEmail = Email.ToUpperInvariant();
            Created = DateTime.UtcNow;
            PasswordHash = passwordHash;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class with the specified login.
        /// </summary>
        public User(string firstName, string lastName, string login, string email, string passwordHash)
        {
            Id = Guid.NewGuid().ToString();
            Login = login;
            UserName = Login;
            NormalizedUserName = UserName.ToUpperInvariant();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            NormalizedEmail = Email.ToUpperInvariant();
            Created = DateTime.UtcNow;
            PasswordHash = passwordHash;
        }

        /// <summary>
        /// Create Login string for the user based on their first and last name.
        /// </summary>
        /// <param name="firstname"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        private string GenerateLogin(string firstname, string lastName) { 

            var firstPart = firstname[0].ToString().ToLower();
            var secondPart = lastName.ToLower();

            return $"{firstPart}{secondPart}";
        }
        
        }

    }
