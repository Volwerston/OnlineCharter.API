using System.Collections.Concurrent;
using Services.Interfaces;

namespace Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly ConcurrentDictionary<string, User> _users 
            = new ConcurrentDictionary<string, User>();

        public User Authenticate(string name, string email)
        {
            if (_users.TryGetValue(email, out var currentUser))
            {
                return currentUser;
            }

            var newUser = User.Create(name, email);
            _users.TryAdd(email, newUser);

            return newUser;
        }
    }
}
