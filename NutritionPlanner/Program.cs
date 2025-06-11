using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using NutritionPlanner.API.Authentication;      
using NutritionPlanner.Application.Services;
using NutritionPlanner.Application.Services.Interfaces;
using NutritionPlanner.DataAccess;
using NutritionPlanner.DataAccess.Repositories;
using NutritionPlanner.DataAccess.Repositories.Interfaces;
using NutritionPlanner.Application.Utilities;

var builder = WebApplication.CreateBuilder(args);

// 1. CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 2. DbContext
builder.Services.AddDbContext<NutritionPlannerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("NutritionPlannerDbContext"))
);

// 3. Репозитории
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IActivityLevelRepository, ActivityLevelRepository>();
builder.Services.AddScoped<IUserGoalRepository, UserGoalRepository>();
builder.Services.AddScoped<IUserProgressRepository, UserProgressRepository>();
builder.Services.AddScoped<IMealPlanRepository, MealPlanRepository>();
builder.Services.AddScoped<IMealPlanItemRepository, MealPlanItemRepository>();
builder.Services.AddScoped<IWeeklyMenuRepository, WeeklyMenuRepository>();
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<IRecipeIngredientRepository, RecipeIngredientRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IGoalTypeRepository, GoalTypeRepository>();
builder.Services.AddScoped<IMealTimeRepository, MealTimeRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();

// 4. Сервисы
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IActivityLevelService, ActivityLevelService>();
builder.Services.AddScoped<IUserGoalService, UserGoalService>();
builder.Services.AddScoped<IUserProgressService, UserProgressService>();
builder.Services.AddScoped<IMealPlanService, MealPlanService>();
builder.Services.AddScoped<IMealPlanItemService, MealPlanItemService>();
builder.Services.AddScoped<IWeeklyMenuService, WeeklyMenuService>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<IRecipeIngredientService, RecipeIngredientService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IGoalTypeService, GoalTypeService>();
builder.Services.AddScoped<IMealTimeService, MealTimeService>();
builder.Services.AddScoped<INutrition, Nutrition>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddHttpContextAccessor();

// 5. Регистрация Authentication и Authorization
builder.Services.AddAuthentication(options =>
{
    // Одна единственная схема: "HeaderScheme"
    options.DefaultAuthenticateScheme = "HeaderScheme";
    options.DefaultChallengeScheme = "HeaderScheme";
})
.AddScheme<AuthenticationSchemeOptions, HeaderAuthenticationHandler>(
    "HeaderScheme", options => { /* никаких дополнительных опций */ }
);

// После AddAuthentication обязательно AddAuthorization
builder.Services.AddAuthorization();

// 6. Controllers, Swagger и т. д.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

// **ВАЖНО**: сначала вызываем Authentication, затем Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
