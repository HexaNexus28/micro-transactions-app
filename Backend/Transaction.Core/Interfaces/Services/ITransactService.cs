using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Core.Dtos.Response;
using Transaction.Core.DTOs.Response;

namespace Transaction.Core.Interfaces.Services
{
    public  interface ITransactService
    {
        Task<ApiResponse<IEnumerable<TransactResponseDto>>> GetAllTransactionsAsync();
        Task<ApiResponse<TransactResponseDto>> GetTransactionByDateAsync(DateTime? date);

        Task<ApiResponse<TransactResponseDto>> GetTransactionWithItemsByDateAsync(DateTime? date);
    }
}
