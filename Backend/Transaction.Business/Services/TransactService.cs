using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Core.Dtos.Response;
using Transaction.Core.DTOs.Response;
using Transaction.Core.Entities;
using Transaction.Core.Interfaces.Repositories;
using Transaction.Core.Interfaces.Services;

namespace Transaction.Business.Services
{
public class TransactService :ITransactService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
       public async Task<ApiResponse<IEnumerable<TransactResponseDto>>> GetAllTransactionsAsync()
        {
            try
            {


                var transacts = await _unitOfWork.Transacts.GetAllAsync();
                if (transacts == null)
                {
                    return ApiResponse<IEnumerable<TransactResponseDto>>.NotFoundResponse(
                            $"Not Found Transact");
                }
                var transactsdto = _mapper.Map<IEnumerable<TransactResponseDto>>(transacts);

                return ApiResponse<IEnumerable<TransactResponseDto>>.SuccessResponse(transactsdto, "Transacts Founds successfully");

            }
            catch (Exception)
            {

                return ApiResponse<IEnumerable<TransactResponseDto>>.ErrorResponse(
                   "An error occurred while retrieving the transact", 500);
            }
        }
       public async Task<ApiResponse<TransactResponseDto>> GetTransactionByDateAsync(DateTime? date)
        {
            try
            {
                if(date == null)  return ApiResponse<TransactResponseDto>.NotFoundResponse(
                            $"Not Found Transact");
                var transact = await _unitOfWork.Transacts.FindAsync(t => t.TransactionDate == date);

                if (transact == null)
                {
                    return ApiResponse<TransactResponseDto>.NotFoundResponse(
                            $"Not Found Transact");
                }
                var transactdto = _mapper.Map<TransactResponseDto>(transact);

                return ApiResponse<TransactResponseDto>.SuccessResponse(transactdto, "Transact Found successfully");
            }
            catch (Exception)
            {
                return ApiResponse<TransactResponseDto>.ErrorResponse(
                   "An error occurred while retrieving the transact", 500);

            }
        }

       public async Task<ApiResponse<TransactResponseDto>> GetTransactionWithItemsByDateAsync(DateTime? date)
        {

            try
            {
                var transactwithItems= await _unitOfWork.Transacts.GetWithIncludesAsync(
                    u => u.TransactionDate == date,
                    nameof(Transact.Items)
                );

                if (transactwithItems== null)
                {
                    return ApiResponse<TransactResponseDto>.NotFoundResponse(
                        $"Transaction with Date {date} not found");
                }

                var transactwithItemsDto = _mapper.Map<TransactResponseDto>(transactwithItems);

                return ApiResponse<TransactResponseDto>.SuccessResponse(
                    transactwithItemsDto,
                    $"Retrieved Transaction with Items");
            }
            catch (Exception)
            {
                return ApiResponse<TransactResponseDto>.ErrorResponse(
                    "An error occurred while retrieving transaction with Items", 500);
            }
        }


    }
}
