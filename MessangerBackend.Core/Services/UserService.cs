using MessangerBackend.Core.Models.
    Exceptions;
using MessangerBackend.Core.Interfaces;
using MessangerBackend.Core.Models;
using MessangerBackend.Core.Models.Exceptions;
using System.Text.RegularExpressions;

namespace MessangerBackend.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository _repository;

        public UserService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<User> Login(string nickname, string password)
        {
            if (nickname == null || password == null)
            {
                throw new UserServiceException("Nickname or password is null");
            }
            if (!IsNicknameValid(nickname))
            {
                throw new UserServiceException("Nickname must be at least 3 characters long.");
            }

            if (!IsPasswordValid(password))
            {
                throw new UserServiceException("Password must be longer than or equals 8 characters and contain at least one letter.");
            }

            var user = _repository.GetAll<User>().FirstOrDefault(u => u.Nickname == nickname);
            if (user == null || !VerifyPassword(password, user.Password))
            {
                throw new UserServiceException("Invalid nickname or password.");
            }

            return user;
        }

        public async Task<User> Register(string nickname, string password)
        {
            if (nickname == null || password == null)
            {
                throw new UserServiceException("Nickname or password is null");
            }
            if (!IsNicknameValid(nickname))
            {
                throw new UserServiceException("Nickname must be at least 3 characters long.");
            }

            if (!IsPasswordValid(password))
            {
                throw new UserServiceException("Password must be longer than or equals 8 characters and contain at least one letter.");
            }

            if (_repository.GetAll<User>().Any(u => u.Nickname == nickname))
            {
                throw new UserServiceException("User with this nickname already exists.");
            }

            var user = new User
            {
                Nickname = nickname,
                Password = HashPassword(password),
                CreatedAt = DateTime.UtcNow,
                LastSeenOnline = DateTime.UtcNow
            };

            await _repository.Add(user);
            return user;
        }

        public Task<User> GetUserById(int id)
        {
            return _repository.GetById<User>(id);
        }

        public IEnumerable<User> GetUsers(int page, int size)
        {
            return _repository.GetAll<User>()
                              .Skip((page - 1) * size)
                              .Take(size)
                              .ToList();
        }

        public IEnumerable<User> SearchUsers(string nickname)
        {
            if (string.IsNullOrWhiteSpace(nickname))
            {
                throw new UserServiceException("Nickname cannot be null or empty.");
            }
            var users = _repository.GetAll<User>()
                              .Where(u => u.Nickname.Contains(nickname.ToLower()))
                              .ToList();

            if (users.Count==0) {
                throw new UserServiceException("Users are empty");
            }
            return users;
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPassword);
        }

        private bool IsPasswordValid(string password)
        {
            return password.Length > 7 && Regex.IsMatch(password, @"[a-zA-Z]");
        }

        private bool IsNicknameValid(string nickname)
        {
            return nickname.Length >= 3;
        }
    }
}
