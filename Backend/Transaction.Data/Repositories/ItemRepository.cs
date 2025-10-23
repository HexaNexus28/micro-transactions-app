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
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        public ItemRepository(AppDbContext _context) : base(_context)
        {

        }

        public async Task<Item> GetItemByIdAsync(int Id)
        {
            var Item = await _dbSet.FirstOrDefaultAsync(i => i.Id == Id);
            if (Item == null)
            {
                throw new Exception("Item non trouvé");
            }
            else return Item;
        }

        public async Task<Item> GetItemByNameAsync(string Name)
        {
            var Item = await _dbSet.FirstOrDefaultAsync(i => i.Name == Name);
            if (Item == null)
            {
                throw new Exception("Item non trouvé");
            }
            else return Item;

        }
    }
}
