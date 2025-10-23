using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Core.Dtos.Response;
using Transaction.Core.DTOs.Response;

namespace Transaction.Core.Interfaces.Services
{
   public  interface IItemService
    {
        Task<ApiResponse<IEnumerable<ItemResponseDto>>> GetAllItemsAsync();
        Task<ApiResponse<ItemResponseDto>> GetItemByIdAsync(int Id);

        Task<ApiResponse<ItemResponseDto>> GetItemByNameAsync(string Name);

    }
}
