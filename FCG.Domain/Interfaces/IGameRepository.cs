using FCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Domain.Interfaces
{
    public interface IGameRepository
    {
        Task AddAsync(Game game, CancellationToken cancellationToken = default);
        Task<Game?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Game>> GetAllAsync(CancellationToken cancellationToken = default);
        Task UpdateAsync(Game game, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
