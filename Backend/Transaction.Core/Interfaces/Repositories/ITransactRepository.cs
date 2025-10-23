using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Core.Dtos.Request;
using Transaction.Core.Dtos.Response;
using Transaction.Core.Entities;

namespace Transaction.Core.Interfaces.Repositories
{
    public  interface ITransactRepository : IGenericRepository<Transact>
    {
        Task CreateAsync(Transact transact);

        Task<IEnumerable<Transact>> GetAllTransactsAsync();

        Task<Transact> GetByUserIdAsync(int userId);
        Task DeleteAsync(DateTime dateTime);

    }
}
