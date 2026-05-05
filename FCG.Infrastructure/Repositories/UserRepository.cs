using Microsoft.EntityFrameworkCore;
using FCG.Domain.Entities;
using FCG.Domain.Interfaces;

namespace FCG.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Persistence.FcgDbContext _db;

        public UserRepository(Persistence.FcgDbContext db) => _db = db;

        public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            _db.Users.Add(user);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _db.Users.FindAsync(new object[] { id }, cancellationToken);
            if (user != null) _db.Users.Remove(user);
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.Users.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _db.Users.Include(u => u.Library).FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            return await _db.Users.Include(u => u.Library)
                .FirstOrDefaultAsync(u => u.Email == username || u.Name == username, cancellationToken);
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            _db.Users.Update(user);
            await Task.CompletedTask;
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _db.Users.AnyAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _db.Users.Include(u => u.Library).FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }
    }
}
