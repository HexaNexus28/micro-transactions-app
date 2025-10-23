using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Core.Entities;
using Transaction.Core.Interfaces.Repositories;
using Transaction.Data.Context;

namespace Transaction.Data.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext _context) : base(_context)
        {

        }


        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _dbSet.FirstOrDefaultAsync(u => u.Id == id);
            
            return user?? throw new InvalidOperationException($"Aucun utilisateur trouvé avec l'ID {id}.") ;
            
        }

       
        public async Task DeleteUserByIdAsync(int id)
        {
            var user = await _dbSet.FindAsync(id);
            if (user != null)
            {
                _dbSet.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

      
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _dbSet.ToListAsync();
        }
    }
}
