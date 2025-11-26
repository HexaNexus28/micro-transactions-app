using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Core.Dtos.Response;
using Transaction.Core.DTOs.Response;

namespace Transaction.Core.Interfaces.Services
{
    public  interface IAuthTokenService
    {
        Task<ApiResponse<IEnumerable<AuthTokenResponseDto>>> GetAllAuthTokenAsync();

        Task<ApiResponse<IEnumerable<AuthTokenResponseDto>>> GetAuthTokenByUserIdAsync(int UserId);

        Task<ApiResponse<bool>> CreateAuthTokenAsync();
    }
}
