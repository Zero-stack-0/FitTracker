using Data.Repository.Interface;
using Data.response;
using Entity;
using Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Service.Dto.Request;
using Service.Dto.Response;
using Service.Helpers;
using Service.Interface;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IUserInformationRepository _userInformationRepository;
        private readonly IFitnessAndnutritionPlansRepository _fitnessAndnutritionPlansRepository;

        public UserService(IConfiguration configuration, IUserRepository userRepository, IUserInformationRepository userInformationRepository, IFitnessAndnutritionPlansRepository fitnessAndnutritionPlansRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _userInformationRepository = userInformationRepository;
            _fitnessAndnutritionPlansRepository = fitnessAndnutritionPlansRepository;
        }

        public async Task<ApiResponse> CreateUser(CreateUserRequest dto)
        {
            if (dto is null)
            {
                return new ApiResponse(null, "Invalid request", (int)HttpStatusCode.BadRequest);
            }

            var getUserByEmail = await _userRepository.GetByEmail(dto.Email);
            if (getUserByEmail is not null)
            {
                return new ApiResponse(null, "Email already exists", StatusCodes.Status400BadRequest);
            }

            var user = new Users
            {
                Email = dto.Email,
                FullName = dto.FullName,
                Password = EncryptString(dto.Password, _configuration["AES_KEY"] ?? ""),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                IsActive = true
            };

            var createdUser = await _userRepository.Create(user);

            if (createdUser == null)
            {
                return new ApiResponse(null, "User creation failed", (int)HttpStatusCode.InternalServerError);
            }

            var userInformation = new UserInformation
            {
                UserId = createdUser.Id.ToString(),
                Age = dto.Age,
                Weight = dto.Weight,
                Gender = dto.Gender,
                Height = dto.Height,
                ActivityLevel = dto.ActivityLevel,
                FitnessGoal = dto.FitnessGoal,
                Location = dto.Location,
                DietType = dto.DietType,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                IsActive = true
            };

            // Create user information
            await _userInformationRepository.Create(userInformation);

            var response = new UserResponse
            {
                Id = createdUser.Id.ToString(),
                FullName = createdUser.FullName,
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
                FullName = user.FullName,
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

            var user = await _userRepository.Login(request.Email, EncryptString(request.Password, _configuration["AES_KEY"] ?? ""));

            if (user == null)
            {
                return new ApiResponse(null, "Invalid email or password", (int)HttpStatusCode.Unauthorized);
            }

            var response = new UserResponse
            {
                Id = user.Id.ToString(),
                FullName = user.FullName,
                Email = user.Email,
                Role = ((Enums.UserRole)user.Role).ToString()
            };

            return new ApiResponse(response, "Login successful", (int)HttpStatusCode.OK);
        }

        public async Task<ApiResponse> GetUserInformation(string email)
        {

            var user = await _userRepository.GetByEmail(email);
            if (user is null)
            {
                return new ApiResponse(null, "Invalid user", StatusCodes.Status400BadRequest);
            }

            var userInformation = await _userInformationRepository.GetByUserId(user.Id);
            if (userInformation is null)
            {
                return new ApiResponse(null, "User information does not exists", StatusCodes.Status400BadRequest);
            }

            var userInfomationResponse = new UserInfomationResponse
            {
                userInformation = userInformation,
                UserId = user.Id,
                email = user.Email,
                fullName = user.FullName
            };

            var basicDietPlan = await _fitnessAndnutritionPlansRepository.GetBasicPlanByUserId(user.Id);
            if (basicDietPlan is not null)
            {
                userInfomationResponse.macroTargets = basicDietPlan.MacroTargets;
            }

            return new ApiResponse(userInfomationResponse, "user information");
        }

        public static string EncryptString(string plainText, string key)
        {
            using var aes = Aes.Create();
            var keyBytes = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
            aes.Key = keyBytes;
            aes.IV = new byte[16]; // All zeros (not secure)
            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }
            return Convert.ToBase64String(ms.ToArray());
        }
    }
}