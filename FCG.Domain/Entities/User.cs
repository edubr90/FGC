using FCG.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }

        public UserRole Role { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public bool IsActive { get; private set; }

        private readonly List<UserGame> _library = new();
        public IReadOnlyCollection<UserGame> Library => _library.AsReadOnly();
        protected User() { }
        public User(string name, string email, string passwordHash)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            CreatedAt = DateTime.UtcNow;
        }
        public void SetName(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Name cannot be empty.");

            Name = username.Trim();
            Touch();
        }
        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be empty.");

            if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Invalid email format.");

            Email = email.Trim().ToLowerInvariant();
            Touch();
        }
        public void UpdatePassword(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash cannot be empty.");

            PasswordHash = passwordHash;
            Touch();
        }

        public void PromoteToAdmin()
        {
            Role = UserRole.Admin;
            Touch();
        }

        public void DemoteToUser()
        {
            Role = UserRole.User;
            Touch();
        }

        public void Deactivate()
        {
            IsActive = false;
            Touch();
        }

        public void Activate()
        {
            IsActive = true;
            Touch();
        }

        public void AddGame(Game game)
        {
            if (_library.Any(ug => ug.GameId == game.Id))
                throw new InvalidOperationException("Game already in library.");

            _library.Add(new UserGame(Id, game.Id));
            Touch();
        }

        private void Touch()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
