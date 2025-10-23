using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Core.Entities;

namespace Transaction.Core.Interfaces.Repositories
{
    public interface IAuthTokenRepository : IGenericRepository<AuthToken>
    {
        Task<AuthToken> CreateAuthTokenAsync(int UserId, User user);
        Task<AuthToken> DeleteAuthTokenAsync(int  UserId);
    }
}
