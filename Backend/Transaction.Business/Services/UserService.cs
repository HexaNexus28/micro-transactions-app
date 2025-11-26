using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Transaction.Core.Dtos.Request;
using Transaction.Core.Dtos.Response;
using Transaction.Core.DTOs.Response;
using Transaction.Core.Entities;
using Transaction.Core.Interfaces.Repositories;
using Transaction.Core.Interfaces.Services;

namespace Transaction.Business.Services
{
    public class UserService : IUserService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

       public async Task<ApiResponse<bool>> CreateUserAsync(RegisterRequestDto userdto)
        {
            try
            {
                if(userdto == null)  
                    return ApiResponse<bool>.ErrorResponse(
                        "Invalid user ID", 400);

                if (GetUserByEmailAsync(userdto.Email) != null)
                {
                    return ApiResponse<bool>.ErrorResponse(
                                            "Email Already exist", 400);
                }
                var user = _mapper.Map<User>(userdto);
                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();


                return ApiResponse<bool>.SuccessResponse(
                        true, $"user {user} created successfully");

            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"[CreateUserAsync ERROR] {ex.Message}\n{ex.StackTrace}");

                return ApiResponse<bool>.ErrorResponse(
                    $"An error occurred: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> LoginAsync(LoginRequestDto userdto)
        {
            try
            {
                if (userdto == null)
                    return ApiResponse<bool>.ErrorResponse(
                        "Invalid user ID", 400);

                if (GetUserByEmailAsync(userdto.Email) != null)
                {
                    return ApiResponse<bool>.ErrorResponse(
                                            "Email Not exist", 400);
                }
                var user = _mapper.Map<User>(userdto);
                var result = await _unitOfWork.Users.GetPassword(userdto.Password);
               
                

                return ApiResponse<bool>.SuccessResponse(
                        true, $"Login successful");

            }
            catch (Exception ex)
            {

                Console.WriteLine($"[CreateUserAsync ERROR] {ex.Message}\n{ex.StackTrace}");

                return ApiResponse<bool>.ErrorResponse(
                    $"An error occurred: {ex.Message}", 500);
            }

        }
        public async Task<ApiResponse<UserResponseDto>> GetUserByIdAsync(int Id)
        {
            try
            {
                if (Id < 0)
                {
                    return ApiResponse<UserResponseDto>.ErrorResponse(
                        "Invalid User ID", 400);
                }

                var user = await _unitOfWork.Users.GetByIdAsync(Id);

                if (user == null)
                {
                    return ApiResponse<UserResponseDto>.NotFoundResponse(
                            $"Not Found AuthToken");

                }
                var userdto = _mapper.Map<UserResponseDto>(user);

                return ApiResponse<UserResponseDto>.SuccessResponse(userdto, "user Founds successfully");
            }
            catch (Exception)
            {
                {
                    return ApiResponse<UserResponseDto>.ErrorResponse(
                   "An error occurred while retrieving the user", 500);

                }


            }
        }
        public async Task<ApiResponse<IEnumerable<UserResponseDto>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _unitOfWork.Users.GetAllAsync();
                var userDtos = _mapper.Map<IEnumerable<UserResponseDto>>(users);

                return ApiResponse<IEnumerable<UserResponseDto>>.SuccessResponse(
                    userDtos,
                    $"Retrieved {userDtos.Count()}  users");
            }
            catch (Exception)
            {
                return ApiResponse<IEnumerable<UserResponseDto>>.ErrorResponse(
                    "An error occurred while retrieving users", 500);
            }

        }
        public async Task<ApiResponse<UserResponseDto>> GetUserWithAuthTokensAsync(int userId)
        {

            try
            {
                var userwithauthtokens = await _unitOfWork.Users.GetWithIncludesAsync(
                    u => u.Id == userId,
                    nameof(User.AuthTokens)
                );

                if (userwithauthtokens == null)
                {
                    return ApiResponse<UserResponseDto>.NotFoundResponse(
                        $"User with ID {userId} not found");
                }

                var userwithauthtokensDto = _mapper.Map<UserResponseDto>(userwithauthtokens);

                return ApiResponse<UserResponseDto>.SuccessResponse(
                    userwithauthtokensDto,
                    $"Retrieved user with AuthTokens");
            }
            catch (Exception)
            {
                return ApiResponse<UserResponseDto>.ErrorResponse(
                    "An error occurred while retrieving user with AuthTokens", 500);
            }

        }

        public async Task<ApiResponse<UserResponseDto>> GetUserWithTransactionsAsync(int userId)
        {
            try
            {
                var userwithtransactions = await _unitOfWork.Users.GetWithIncludesAsync(
                    u => u.Id == userId,
                    nameof(User.Transactions)
                );

                if (userwithtransactions == null)
                {
                    return ApiResponse<UserResponseDto>.NotFoundResponse(
                        $"User with ID {userId} not found");
                }

                var userwithtransactionsDto = _mapper.Map<UserResponseDto>(userwithtransactions);

                return ApiResponse<UserResponseDto>.SuccessResponse(
                    userwithtransactionsDto,
                    $"Retrieved user with Transactions");
            }
            catch (Exception)
            {
                return ApiResponse<UserResponseDto>.ErrorResponse(
                    "An error occurred while retrieving user with AuthTokens", 500);
            }
        }

        public async Task<ApiResponse<UserResponseDto>> GetUserByNameAsync(string Name)
        {
            try
            {
                if (Name == null)
                    return ApiResponse<UserResponseDto>.NotFoundResponse(
                        $"User with Name {Name} not found");
                var user = await _unitOfWork.Users.FindAsync(u => u.UserName == Name);

                if (user == null)
                {
                    return ApiResponse<UserResponseDto>.NotFoundResponse(
                            $"Not Found User {Name}");

                }
                var userdto = _mapper.Map<UserResponseDto>(user);

                return ApiResponse<UserResponseDto>.SuccessResponse(userdto, "user Founds successfully");
            }
            catch (Exception)
            {
                {
                    return ApiResponse<UserResponseDto>.ErrorResponse(
                   "An error occurred while retrieving the user", 500);

                }

            }
        }

          public async  Task<ApiResponse<UserResponseDto>> GetUserByEmailAsync(string Email)
        {
            try
            {
                if (Email == null)
                    return ApiResponse<UserResponseDto>.NotFoundResponse(
                        $"User with Name {Email} not found");
                var user = await _unitOfWork.Users.FindAsync(u => u.Email == Email);

                if (user == null)
                {
                    return ApiResponse<UserResponseDto>.NotFoundResponse(
                            $"Not Found AuthToken");

                }
                var userdto = _mapper.Map<UserResponseDto>(user);

                return ApiResponse<UserResponseDto>.SuccessResponse(userdto, "user Founds successfully");
            }
            catch (Exception)
            {
                {
                    return ApiResponse<UserResponseDto>.ErrorResponse(
                   "An error occurred while retrieving the user", 500);

                }

            }
        }

            

    }
    
}
