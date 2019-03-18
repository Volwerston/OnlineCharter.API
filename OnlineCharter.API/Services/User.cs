using Utils;

namespace Services.Interfaces
{
    public class User
    {
        public string Name { get; }
        public string Email { get; }

        public User(string name, string email)
        {
            Ensure.NotNullOrEmpty(name, nameof(name));
            Ensure.NotNullOrEmpty(email, nameof(email));

            Name = name;
            Email = email;
        }

        public static User Create(string name, string email)
            => new User(name, email);
    }
}
