using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Core.Interfaces.Repositories;
using Transaction.Data.Context;
using Transaction.Data.Repositories;

namespace Transaction.Data.UnitOfWork
{
   public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

        // Repositories lazy loading
        private IUserRepository? _userRepository;
        private IAuthTokenRepository? _authTokenRepository;
        private IItemRepository? _itemRepository;

        private ITransactRepository? _transactRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IUserRepository Users
        {
            get
            {
                _userRepository ??= new UserRepository(_context);
                return _userRepository;
            }
        }

        public IAuthTokenRepository AuthTokens
        {
            get
            {
                _authTokenRepository ??= new AuthTokenRepository(_context);
                return _authTokenRepository;
            }
        }

        public ITransactRepository Transacts
        {
            get
            {
                _transactRepository ??= new TransactRepository(_context);
                return _transactRepository;
            }
        }

        public IItemRepository Items
        {
            get
            {
                _itemRepository ??= new ItemRepository(_context);
                return _itemRepository;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _context.Dispose();
            }
        }
    }
}
