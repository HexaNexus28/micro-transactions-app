using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Core.Dtos.Request;
using Transaction.Core.Entities;
using Transaction.Core.Interfaces.Repositories;
using Transaction.Data.Context;

namespace Transaction.Data.Repositories
{
   public class TransactRepository :GenericRepository<Transact>, ITransactRepository
    {
        public TransactRepository(AppDbContext _context) : base(_context)
        {

        }

        public async Task CreateAsync(Transact transact )
        {
            if (transact == null)
            {
                throw new ArgumentNullException(nameof(transact));
            }
            else
            {
                await _dbSet.AddAsync(transact);
            }
        }
        public async Task<IEnumerable<Transact>> GetAllTransactsAsync()
        {
            return await _dbSet.ToListAsync();

        }

        public async Task<Transact> GetByUserIdAsync( int UserId)
        {
            var transact = await _dbSet.FirstOrDefaultAsync(x => x.UserId == UserId);
            if (transact == null)
            {
                throw new InvalidOperationException("Transaction introuvable pour la date spécifiée.");
            }
            else
            {
                return transact;
            }

            }
        public async Task DeleteAsync(DateTime dateTime)
        {
            var transact = await _dbSet.FirstOrDefaultAsync(u=>u.TransactionDate == dateTime);
            if (transact == null)
            {
                throw new InvalidOperationException("Transaction introuvable pour la date spécifiée.");
            }else
            {
             _dbSet.Remove(transact);
            }
                
        }

    }
}
