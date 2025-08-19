using Data.Repository.Interface;
using Data.response;
using Entity;
using Entity.DbModels;
using Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Service.Dto.Request;
using Service.Dto.Response;
using Service.Helpers;
using Service.Helpers.EmailNotification;
using Service.Interface;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using static Entity.Enums;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IUserInformationRepository _userInformationRepository;
        private readonly IFitnessAndnutritionPlansRepository _fitnessAndnutritionPlansRepository;
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly SendEmailNotificationService sendEmailNotificationService;
        private readonly IEmailLoggerRepository _emailLoggerRepository;
        private static readonly Regex AllowedEmailRegex = new Regex(
    @"^[a-zA-Z0-9._%+-]+@(gmail\.com|yahoo\.com|outlook\.com|hotmail\.com|live\.com|icloud\.com|aol\.com|protonmail\.com|zoho\.com|mail\.com)$",
    RegexOptions.IgnoreCase | RegexOptions.Compiled

);

        public UserService(IConfiguration configuration, IUserRepository userRepository, IUserInformationRepository userInformationRepository,
        IFitnessAndnutritionPlansRepository fitnessAndnutritionPlansRepository, IEmailTemplateRepository emailTemplateRepository, SendEmailNotificationService sendEmailNotificationService,
        IEmailLoggerRepository emailLoggerRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _userInformationRepository = userInformationRepository;
            _fitnessAndnutritionPlansRepository = fitnessAndnutritionPlansRepository;
            _emailTemplateRepository = emailTemplateRepository;
            _emailLoggerRepository = emailLoggerRepository;
            this.sendEmailNotificationService = sendEmailNotificationService;
        }

        #region Api Methods

        public ApiResponse IsEmailValid(string email)
        {
            return new ApiResponse(IsValidEmail(email));
        }
        public async Task<ApiResponse> CreateUser(CreateUserRequest dto)
        {
            if (dto is null || string.IsNullOrWhiteSpace(_configuration["AppSettings:BaseUrl"]))
            {
                return new ApiResponse(null, "Invalid request", (int)HttpStatusCode.BadRequest);
            }

            var getUserByEmail = await _userRepository.GetByEmail(dto.Email);
            if (getUserByEmail is not null)
            {
                return new ApiResponse(null, "Email already exists", StatusCodes.Status400BadRequest);
            }

            if (!IsValidEmail(dto.Email))
            {
                return new ApiResponse(null, "Please enter valid domain", StatusCodes.Status400BadRequest);
            }

            var user = new Users
            {
                Email = dto.Email,
                FullName = dto.FullName,
                Password = EncryptString(dto.Password, _configuration["AES_KEY"] ?? ""),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                IsActive = true,
                IsEmailVerified = false,
                EmailVerificationCode = GenerateVerificationCode()
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

            var (emailBody, emailSubject) = await GetEmailTemplateSubjectAndBody(EMAIL_TEMPLATE_TYPE.Verification, user);

            bool isEmailSent = sendEmailNotificationService.SendEmail(user.Email, user.FullName, emailSubject, emailBody);

            var emilLogger = new EmailLogger
            {
                UserId = user.Id,
                EmailTemplateType = EMAIL_TEMPLATE_TYPE.Verification,
                CreatedAt = DateTime.UtcNow,
                IsEmailSent = isEmailSent
            };
            await _emailLoggerRepository.Create(emilLogger);

            if (isEmailSent)
                await _userRepository.UpdateIsEmailSentFlag(user);

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
                Role = ((Enums.UserRole)user.Role).ToString(),
                IsEmailVerified = user.IsEmailVerified
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
                fullName = user.FullName,
                IsEmailVerified = user.IsEmailVerified
            };

            var basicDietPlan = await _fitnessAndnutritionPlansRepository.GetBasicPlanByUserId(user.Id);
            if (basicDietPlan is not null)
            {
                userInfomationResponse.macroTargets = basicDietPlan.MacroTargets;
            }

            return new ApiResponse(userInfomationResponse, "user information");
        }

        public async Task<ApiResponse> VerifyEmail(string code)
        {
            var getUserByCode = await _userRepository.GetByVerificationCode(code);
            if (getUserByCode is null)
            {
                return new ApiResponse(null, "Invalid verification code", StatusCodes.Status400BadRequest);
            }

            if (getUserByCode.IsEmailVerified)
            {
                return new ApiResponse(null, "Email is already verified");
            }

            getUserByCode.IsEmailVerified = true;
            getUserByCode.EmailVerifiedAt = DateTime.UtcNow;

            await _userRepository.UpdateEmailVerification(getUserByCode);

            return new ApiResponse(null, "User verified sucessfully", StatusCodes.Status200OK);
        }

        public async Task<ApiResponse> SentEmailVerificationLink(string userId)
        {
            var user = await _userRepository.GetById(userId);
            if (user is null)
            {
                return new ApiResponse(null, "Invalid requestor", StatusCodes.Status400BadRequest);
            }

            if (user.IsEmailVerified)
            {
                return new ApiResponse(null, "User's email already verified", StatusCodes.Status400BadRequest);
            }

            var emailSentTodayForVerification = await _emailLoggerRepository.GetByUserIdAndType(user.Id, EMAIL_TEMPLATE_TYPE.Verification);
            if (emailSentTodayForVerification.Count > 3)
            {
                return new ApiResponse(null, "You have already re-sent the email verification 3 times. Please try again tomorrow.", StatusCodes.Status400BadRequest);
            }

            var (emailBody, emailSubject) = await GetEmailTemplateSubjectAndBody(EMAIL_TEMPLATE_TYPE.Verification, user);

            bool isEmailSent = sendEmailNotificationService.SendEmail(user.Email, user.FullName, emailSubject, emailBody);

            var emilLogger = new EmailLogger
            {
                UserId = user.Id,
                EmailTemplateType = EMAIL_TEMPLATE_TYPE.Verification,
                CreatedAt = DateTime.UtcNow,
                IsEmailSent = isEmailSent
            };
            await _emailLoggerRepository.Create(emilLogger);

            if (!user.IsEmailVerificationEmailSent && isEmailSent)
                await _userRepository.UpdateIsEmailSentFlag(user);

            return new ApiResponse(null, "Email verification sent sucessfully");
        }
        #endregion

        #region Private Methods
        private static string EncryptString(string plainText, string key)
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

        private static string GenerateVerificationCode()
        {
            string guidPart = Guid.NewGuid().ToString("N");
            long ticksPart = DateTime.UtcNow.Ticks;
            Random random = new Random();
            int randomPart = random.Next(1000, 9999);

            string uniqueString = $"{guidPart}_{ticksPart}_{randomPart}";

            return uniqueString;
        }

        private async Task<(string, string)> GetEmailTemplateSubjectAndBody(EMAIL_TEMPLATE_TYPE type, Users user)
        {
            var emailTemplate = await _emailTemplateRepository.GetByType(type);
            if (emailTemplate is null)
            {
                return (string.Empty, string.Empty);
            }

            string verifyUrl = $"{_configuration["AppSettings:BaseUrl"]}/api/User/verify-email?code={user.EmailVerificationCode}";
            var body = ReplaceEmailContentForVerification(emailTemplate.Body, user.FullName, verifyUrl);

            return (body, emailTemplate.Subject);
        }

        private string ReplaceEmailContentForVerification(string bodyVerify, string fullName, string verificationUrl)
        {
            if (string.IsNullOrEmpty(bodyVerify))
                return string.Empty;

            string unescapedBody = System.Text.RegularExpressions.Regex.Unescape(bodyVerify);

            string cleanUrl = verificationUrl?.Trim('"', ' ');

            string emailBody = unescapedBody
                .Replace("{{UserName}}", fullName ?? string.Empty)
                .Replace("{{VerificationLink}}", cleanUrl ?? string.Empty);

            return emailBody;
        }



        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return AllowedEmailRegex.IsMatch(email);
        }
        #endregion
    }
}