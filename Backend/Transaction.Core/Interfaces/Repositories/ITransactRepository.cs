using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Core.Dtos.Request;
using Transaction.Core.Entities;

namespace Transaction.Core.Interfaces.Repositories
{
    public  interface ITransactRepository : IGenericRepository<Transact>
    {
        Task<TransactRequestDto> CreateAsync(TransactRequestDto dto);

        Task<Transact> GetAllTransactAsync(int id);

        Task<Transact> GetTransactByUserId(int userId);
        Task DeleteAsync(int id);

    }
}
