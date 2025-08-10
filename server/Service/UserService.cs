using Data.Repository.Interface;
using Entity;
using Entity.Models;
using Service.Dto.Request;
using Service.Dto.Response;
using Service.Helpers;
using Service.Interface;
using System.Net;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ApiResponse> CreateUser(CreateUserRequest dto)
        {
            if (dto is null)
            {
                return new ApiResponse(null, "Invalid request", (int)HttpStatusCode.BadRequest);
            }

            var user = new Users
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Password = dto.Password,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                IsActive = true
            };

            var createdUser = await _userRepository.Create(user);

            if (createdUser == null)
            {
                return new ApiResponse(null, "User creation failed", (int)HttpStatusCode.InternalServerError);
            }

            var response = new UserResponse
            {
                Id = createdUser.Id.ToString(),
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                Email = createdUser.Email,
                Role = ((Enums.UserRole)createdUser.Role).ToString()
            };

            return new ApiResponse(response, "User created successfully", (int)HttpStatusCode.Created);
        }

        public async Task<ApiResponse> GetByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return new ApiResponse(null, "Email cannot be null or empty", (int)HttpStatusCode.BadRequest);
            }

            var user = await _userRepository.GetByEmail(email);

            if (user == null)
            {
                return new ApiResponse(null, "User not found", (int)HttpStatusCode.NotFound);
            }

            var response = new UserResponse
            {
                Id = user.Id.ToString(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = ((Enums.UserRole)user.Role).ToString()
            };

            return new ApiResponse(response, "User retrieved successfully", (int)HttpStatusCode.OK);
        }

        public async Task<ApiResponse> Login(LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return new ApiResponse(null, "Invalid login data", (int)HttpStatusCode.BadRequest);
            }

            var user = await _userRepository.Login(request.Email, request.Password);

            if (user == null)
            {
                return new ApiResponse(null, "Invalid email or password", (int)HttpStatusCode.Unauthorized);
            }

            var response = new UserResponse
            {
                Id = user.Id.ToString(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = ((Enums.UserRole)user.Role).ToString()
            };

            return new ApiResponse(response, "Login successful", (int)HttpStatusCode.OK);
        }
    }
}