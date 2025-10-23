using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Core.Entities;

namespace Transaction.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task <User> GetUserByIdAsync(int id);
        Task DeleteUserByIdAsync(int id);
        Task<List<User>> GetAllUsersAsync();

       
    }
}
