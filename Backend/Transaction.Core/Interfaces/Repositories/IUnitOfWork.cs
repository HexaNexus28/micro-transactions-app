using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transaction.Core.Interfaces.Repositories
{
    // <summary>
    /// Interface pour le pattern Unit of Work
    /// Coordonne le travail de plusieurs repositories
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IAuthTokenRepository AuthTokens { get; }

        IItemRepository Items { get; }

        ITransactRepository Transacts{ get; }

        IUserRepository Users { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }

}
