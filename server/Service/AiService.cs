using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Data.Repository.Interface;
using Entity.DbModels;
using Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Service.Dto.Request;
using Service.Helpers;
using Service.Interface;
using static Entity.Enums;

namespace Service
{
    public class AiService : IAiService
    {
        private readonly IAiPromptRepository _aiPromptRepository;
        private readonly IUserInformationRepository _userInformationRepository;
        private readonly IFitnessAndnutritionPlansRepository _fitnessAndnutritionPlansRepository;
        private readonly IConfiguration _configuration;
        private readonly static HttpClient client = new HttpClient();

        public AiService(IConfiguration configuration, IAiPromptRepository aiPromptRepository, IUserInformationRepository userInformationRepository, IFitnessAndnutritionPlansRepository fitnessAndnutritionPlansRepository)
        {
            _aiPromptRepository = aiPromptRepository;
            _userInformationRepository = userInformationRepository;
            _fitnessAndnutritionPlansRepository = fitnessAndnutritionPlansRepository;
            _configuration = configuration;
        }
        public async Task<ApiResponse> GenerateFitnessPlan(GenerateFitnessPlanRequest req)
        {
            var user = await _userInformationRepository.GetByUserId(req.UserId ?? string.Empty);
            if (user is null)
            {
                return new ApiResponse(null, "Invalid userid", StatusCodes.Status400BadRequest);
            }

            if (string.IsNullOrWhiteSpace(req.Location))
            {
                req.Location = "Mumbai";
            }

            if ((int)req.DietType == 0)
            {
                req.DietType = DIET_TYPE.VEG;
            }

            var (prompt, doesErrorOccured) = await GetReplacedPrompt(user, req.Location, req.DietType);
            if (!doesErrorOccured)
            {
                return new ApiResponse(null, prompt, StatusCodes.Status500InternalServerError);
            }

            var responseContent = await CallGeminiFlashAsync(prompt, _configuration["Gemini_AI_KEY"] ?? string.Empty);

            await _aiPromptRepository.SaveAiResponse(new AiResponse
            {
                Type = AiPromptType.Default,
                Response = responseContent
            });

            var (fitnessPlan, nutritionPlan) = ExtractPlansFromGeminiResponse(responseContent);
            if (fitnessPlan is null && nutritionPlan is null)
            {
                return new ApiResponse(null, "Failed to generate fitness plan", StatusCodes.Status500InternalServerError);
            }

            var fitnessAndNutritionPlan = new FitnessAndNutritionPlan
            {
                UserInformationId = user.Id.ToString(),
                FitnessPlan = fitnessPlan ?? new FitnessPlan(),
                NutritionPlan = nutritionPlan ?? new NutritionPlan(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            var createdPlan = await _fitnessAndnutritionPlansRepository.Create(fitnessAndNutritionPlan);
            if (createdPlan is null)
            {
                return new ApiResponse(null, "Failed to create fitness and nutrition plan", StatusCodes.Status500InternalServerError);
            }
            return new ApiResponse(createdPlan, "Fitness and nutrition plan created successfully", StatusCodes.Status200OK);
        }
        private async Task<(string, bool)> GetReplacedPrompt(UserInformation user, string location, DIET_TYPE type)
        {
            var aiPrompt = await _aiPromptRepository.GetById(AiPromptType.Default);
            if (aiPrompt == null)
            {
                return ("AI Prompt not found.", false);
            }

            return (aiPrompt.Prompt
            .Replace("{GENDER}", user.Gender.ToString())
            .Replace("{HEIGHT}", user.Height.ToString())
            .Replace("{WEIGHT}", user.Weight.ToString())
            .Replace("{AGE}", user.Age.ToString())
            .Replace("{ACTIVITY_LEVEL}", user.ActivityLevel.ToString())
            .Replace("{FITNESSGOAL}", user.FitnessGoal.ToString())
            .Replace("{DIET_NAT}", type.ToString())
            .Replace("{LOCATION}", location), true);
        }
        private static async Task<string> CallGeminiFlashAsync(string prompt, string apiKey)
        {
            var requestUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("x-goog-api-key", apiKey);

            try
            {
                var response = await client.PostAsync(requestUrl, content);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }

        }
        private (FitnessPlan?, NutritionPlan?) ExtractPlansFromGeminiResponse(string jsonResponse)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            try
            {
                using JsonDocument document = JsonDocument.Parse(jsonResponse);
                JsonElement root = document.RootElement;

                string contentText = string.Empty;
                if (root.TryGetProperty("candidates", out JsonElement candidates) && candidates.GetArrayLength() > 0)
                {
                    if (candidates[0].TryGetProperty("content", out JsonElement content))
                    {
                        if (content.TryGetProperty("parts", out JsonElement parts) && parts.GetArrayLength() > 0)
                        {
                            if (parts[0].TryGetProperty("text", out JsonElement textElement))
                            {
                                contentText = textElement.GetString();
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(contentText))
                {
                    return (null, null);
                }

                string pattern = @"```json\n([\s\S]*?)```";
                var matches = Regex.Matches(contentText, pattern);

                if (matches.Count == 2)
                {
                    string fitnessPlanJson = matches[0].Groups[1].Value.Trim();
                    string nutritionPlanJson = matches[1].Groups[1].Value.Trim();

                    var fitnessPlan = System.Text.Json.JsonSerializer.Deserialize<FitnessPlan>(fitnessPlanJson, options);
                    var nutritionPlan = System.Text.Json.JsonSerializer.Deserialize<NutritionPlan>(nutritionPlanJson, options);

                    return (fitnessPlan, nutritionPlan);

                }
                else
                {
                    return (null, null);
                }
            }
            catch (System.Text.Json.JsonException ex)
            {
                //logging the error or handling it as needed
                Console.WriteLine($"JSON parsing error: {ex.Message}");
                return (null, null);
            }
        }
    }
}