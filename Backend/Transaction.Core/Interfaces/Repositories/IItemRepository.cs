using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Core.Entities;

namespace Transaction.Core.Interfaces.Repositories
{
    public  interface IItemRepository : IGenericRepository<Item>
    {
        Task<Item> GetItemByIdAsync (int id);
        Task<Item> GetItemByNameAsync (string name);
       
    }
}
