using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Transaction.Core.Configuration;
using Transaction.Core.Dtos.Request;
using Transaction.Core.Dtos.Response;
using Transaction.Core.DTOs.Response;
using Transaction.Core.Entities;
using Transaction.Core.Interfaces.Repositories;
using Transaction.Core.Interfaces.Services;
using Transaction.Core.Services;

namespace Transaction.Business.Services
{
    public class UserService : IUserService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IJwtService jwtService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
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

        public async Task<ApiResponse<UserResponseDto>> LoginAsync(LoginRequestDto userdto)
        {
            try
            {
                if (userdto == null)
                    return ApiResponse<UserResponseDto>.ErrorResponse(
                        "Invalid login data", 400);

                var user = (await _unitOfWork.Users.FindAsync(u => u.Email == userdto.Email)).FirstOrDefault();
                
                if (user == null)
                    return ApiResponse<UserResponseDto>.ErrorResponse("User not found", 404);

                // Vérifier le mot de passe (simplifié, utiliser BCrypt en production)
                if (user.PasswordHash != userdto.Password)
                    return ApiResponse<UserResponseDto>.ErrorResponse("Invalid password", 401);

                var userDto = _mapper.Map<UserResponseDto>(user);
                return ApiResponse<UserResponseDto>.SuccessResponse(userDto, "Login successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LoginAsync ERROR] {ex.Message}\n{ex.StackTrace}");
                return ApiResponse<UserResponseDto>.ErrorResponse(
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

        public async Task<ApiResponse<UserResponseDto>> LoginWithTokenAsync(LoginRequestDto loginDto)
        {
            try
            {
                if (loginDto == null)
                    return ApiResponse<UserResponseDto>.ErrorResponse("Invalid login data", 400);

                var user = (await _unitOfWork.Users.FindAsync(u => u.Email == loginDto.Email)).FirstOrDefault();
                
                if (user == null)
                    return ApiResponse<UserResponseDto>.ErrorResponse("User not found", 404);

                // Vérifier le mot de passe (à adapter avec hashage)
                if (user.PasswordHash != loginDto.Password) // Simplifié, utiliser BCrypt en production
                    return ApiResponse<UserResponseDto>.ErrorResponse("Invalid password", 401);

                var token = _jwtService.GenerateToken(user);
                var userDto = _mapper.Map<UserResponseDto>(user);
                
                // Ajouter le token à la réponse
                userDto.Token = token;

                return ApiResponse<UserResponseDto>.SuccessResponse(userDto, "Login successful");
            }
            catch (Exception)
            {
                return ApiResponse<UserResponseDto>.ErrorResponse("An error occurred during login", 500);
            }
        }

    }
    
}
