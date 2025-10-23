using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Core.Dtos.Response;
using Transaction.Core.DTOs.Response;

namespace Transaction.Core.Interfaces.Services
{
    public  interface IUserService
    {
        Task<ApiResponse<UserResponseDto>> GetUserByIdAsync(int Id);

        Task<ApiResponse<IEnumerable<UserResponseDto>>> GetAllUsersAsync();

        Task<ApiResponse<UserResponseDto>> GetUserWithAuthTokensAsync();

        Task<ApiResponse<UserResponseDto>> GetUsersWithTransactionsAsync();

        Task<ApiResponse<UserResponseDto>> GetUserByNameAsync(string Name);

        Task<ApiResponse<UserResponseDto>> GetUserByEmailAsync (string  Email);

        Task<ApiResponse<bool>> UpdateUserAsync (UserResponseDto User);
    }
}
