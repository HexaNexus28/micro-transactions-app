using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Core.Dtos.Response;
using Transaction.Core.DTOs.Response;
using Transaction.Core.Interfaces.Repositories;
using Transaction.Core.Interfaces.Services;

namespace Transaction.Business.Services
{
    public class AuthTokenService : IAuthTokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuthTokenService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ApiResponse<IEnumerable<AuthTokenResponseDto>>> GetAllAuthTokenAsync()
        {
            try
            {


                var authtokens = await _unitOfWork.AuthTokens.GetAllAsync();
                if (authtokens == null)
                {
                    return ApiResponse<IEnumerable<AuthTokenResponseDto>>.NotFoundResponse(
                            $"Not Found AuthToken");
                }
                var authtokensdto = _mapper.Map<IEnumerable<AuthTokenResponseDto>>(authtokens);

                return ApiResponse<IEnumerable<AuthTokenResponseDto>>.SuccessResponse(authtokensdto, "AuthTokens Founds successfully");

            }
            catch (Exception ) {

                return ApiResponse<IEnumerable<AuthTokenResponseDto>>.ErrorResponse(
                   "An error occurred while retrieving the authtoken", 500);
            }
           
        }
        public async Task<ApiResponse<IEnumerable<AuthTokenResponseDto>>> GetAuthTokenByUserIdAsync(int UserId)
        {

            try
            {
                if (UserId < 0)
                {
                    return ApiResponse<IEnumerable<AuthTokenResponseDto>>.ErrorResponse(
                        "Invalid User ID", 400);
                }
                var authtokens = await _unitOfWork.AuthTokens.FirstOrDefaultAsync(u=>u.UserId == UserId);

                if (authtokens == null)
                {
                    return ApiResponse<IEnumerable<AuthTokenResponseDto>>.NotFoundResponse(
                            $"Not Found AuthToken");

                }
                var authtokensdto = _mapper.Map<IEnumerable<AuthTokenResponseDto>>(authtokens);

                return ApiResponse<IEnumerable<AuthTokenResponseDto>>.SuccessResponse(authtokensdto, "AuthTokens Founds successfully");
            }
            catch (Exception)
            {
                {
                    return ApiResponse<IEnumerable<AuthTokenResponseDto>>.ErrorResponse(
                   "An error occurred while retrieving the user", 500);

                }


            }
        }


    }
}
