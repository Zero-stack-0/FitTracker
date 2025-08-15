using System.Text;
using AspNetCoreRateLimit;
using Data;
using Data.Repository;
using Data.Repository.Interface;
using Entity.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Service;
using Service.Interface;
using Webservices.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

#region Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAiService, AiService>();
builder.Services.AddScoped<IUserFoodLogService, UserFoodLogService>();
builder.Services.AddScoped<IFoodMacrosService, FoodMacrosService>();
builder.Services.AddScoped<IUserDietPlanService, UserDietPlanService>();
#endregion

#region Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserInformationRepository, UserInformationRepository>();
builder.Services.AddScoped<IAiPromptRepository, AiPromptRepository>();
builder.Services.AddScoped<IUserFoodLogRepository, UserFoodLogRepository>();
builder.Services.AddScoped<IFitnessAndnutritionPlansRepository, FitnessAndnutritionPlansRepository>();
builder.Services.AddScoped<IIndianFoodMacrosRepository, IndianFoodMacrosRepository>();
#endregion

#region Helpers
builder.Services.AddSingleton<FitTrackerDbContext>();
builder.Services.AddSingleton<GenerateJwtToken>();
builder.Services.AddScoped<UserProfile>();
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();


#endregion

builder.Services.Configure<MongoDbMappingConfiguration>(
    builder.Configuration.GetSection("MongoConfig"));

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");

builder.Services.Configure<JwtSettings>(jwtSettingsSection);

var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings?.Issuer,
        ValidAudience = jwtSettings?.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Secret ?? ""))
    };
});
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAllOrigins");
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapControllers();
}
app.UseIpRateLimiting();
app.UseHttpsRedirection();
// var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
// app.Urls.Add($"http://*:{port}");
app.Run();