using System;
using Utils;

namespace Services.Interfaces
{
    public class User
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Email { get; }

        public User(Guid id, string name, string email)
        {
            Ensure.NotNullOrEmpty(name, nameof(name));
            Ensure.NotNullOrEmpty(email, nameof(email));

            Id = id;
            Name = name;
            Email = email;
        }

        public static User Create(string name, string email)
            => new User(Guid.NewGuid(), name, email);
    }
}
