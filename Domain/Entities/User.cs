using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Entities
{
    public  class User
    {
        public Guid Id { get; private set; }

        public string Login { get; private set; }
        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Email { get; private set; }
        public DateTime Created { get; private set; }

        public ICollection<Project> Projects { get; private set; } = new List<Project>();

        private User() { }

        public User(string firstName, string lastName, string email)
        {
            Id = Guid.NewGuid();
            Login loginObj = new Login(firstName, lastName);
            Login = loginObj.Value;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Created = DateTime.UtcNow;
        }
    }

    public class  Login 
    {
        public string Value { get; private set; }

        public Login(string firstname, string lastName)
        {
            Value = Login.CreateLogin(firstname, lastName );
        }

        private Login() { }

        public static string CreateLogin(string firstname, string lastName)
        {
            var firstPart = firstname[0].ToString().ToLower();
            var secondPart = lastName.ToLower();

            return $"{firstPart}{secondPart}";
        }

    }
}
