using Service.Dto.Request;
using Service.Helpers;

namespace Service.Interface
{
    public interface IUserService
    {
        Task<ApiResponse> CreateUser(CreateUserRequest dto);
        Task<ApiResponse> GetByEmail(string email);
        Task<ApiResponse> Login(LoginRequest request);
        Task<ApiResponse> GetUserInformation(string email);
        Task<ApiResponse> VerifyEmail(string code);
    }
}