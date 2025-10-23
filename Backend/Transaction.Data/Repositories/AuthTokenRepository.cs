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
    public class AuthTokenRepository : GenericRepository<AuthToken>, IAuthTokenRepository
    {
        public AuthTokenRepository(AppDbContext _context) : base(_context)
        {

        }

        public async Task CreateAuthTokenAsync(int Id, User user)
        {
            var authtoken = new AuthToken
            {
                EmissionDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddHours(1),
                UserId = Id,
                User = user
            };
            await _dbSet.AddAsync(authtoken);

        }

        
    }
}
