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
  public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

       public async Task<ApiResponse<IEnumerable<ItemResponseDto>>> GetAllItemsAsync()
        {
            try
            {


                var items = await _unitOfWork.Items.GetAllAsync();
                if (items == null)
                {
                    return ApiResponse<IEnumerable<ItemResponseDto>>.NotFoundResponse(
                            $"Not Found Items");
                }
                var itemsdto = _mapper.Map<IEnumerable<ItemResponseDto>>(items);

                return ApiResponse<IEnumerable<ItemResponseDto>>.SuccessResponse(itemsdto, "items Founds successfully");

            }
            catch (Exception)
            {

                return ApiResponse<IEnumerable<ItemResponseDto>>.ErrorResponse(
                   "An error occurred while retrieving the authtoken", 500);
            }
        }
        public async Task<ApiResponse<ItemResponseDto>> GetItemByIdAsync(int Id)
        {
            try
            {
                if (Id < 0)
                {
                    return ApiResponse<ItemResponseDto>.ErrorResponse(
                        "Invalid Item ID", 400);
                }
                var item = await _unitOfWork.Items.GetByIdAsync(Id);

                if (item == null)
                {
                    return ApiResponse<ItemResponseDto>.NotFoundResponse(
                            $"Not Found AuthToken");

                }
                var Itemdto = _mapper.Map<ItemResponseDto>(item);

                return ApiResponse<ItemResponseDto>.SuccessResponse(Itemdto, "Item Found successfully");
            }
            catch (Exception)
            {
                {
                    return ApiResponse<ItemResponseDto>.ErrorResponse(
                   "An error occurred while retrieving the Item", 500);

                }
            }
        }

       public  async Task<ApiResponse<ItemResponseDto>> GetItemByNameAsync(string Name)
        {
            try
            {
                if (Name == null)
                    return ApiResponse<ItemResponseDto>.NotFoundResponse(
                        $"Item with Name {Name} not found");
                var Item = await _unitOfWork.Items.FindAsync(u => u.Name == Name);

                if (Item == null)
                {
                    return ApiResponse<ItemResponseDto>.NotFoundResponse(
                            $"Not Found AuthToken");

                }
                var Itemdto = _mapper.Map<ItemResponseDto>(Item);

                return ApiResponse<ItemResponseDto>.SuccessResponse(Itemdto, "Item Founds successfully");
            }
            catch (Exception)
            {
                {
                    return ApiResponse<ItemResponseDto>.ErrorResponse(
                   "An error occurred while retrieving the Item", 500);

                }

            }

        }

       
    }
}
