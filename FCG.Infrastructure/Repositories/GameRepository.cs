using Microsoft.EntityFrameworkCore;
using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FCG.Infrastructure.Persistence;

namespace FCG.Infrastructure.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly FcgDbContext _context;

        public GameRepository(FcgDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Game game, CancellationToken cancellationToken = default)
        {
            await _context.Games.AddAsync(game, cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var game = await GetByIdAsync(id, cancellationToken);

            if (game is not null)
                _context.Games.Remove(game);
        }

        public async Task<IEnumerable<Game>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Games.Where(_ => _.IsActive).ToListAsync(cancellationToken);
        }

        public async Task<Game?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Games.FirstOrDefaultAsync(_ => _.Id == id && _.IsActive, cancellationToken);
        }

        public Task UpdateAsync(Game game, CancellationToken cancellationToken = default)
        {
            _context.Games.Update(game);
            return Task.CompletedTask;
        }
    }
}
