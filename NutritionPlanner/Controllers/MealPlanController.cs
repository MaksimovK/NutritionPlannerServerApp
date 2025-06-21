using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutritionPlanner.Application.Services;
using NutritionPlanner.Application.Services.Interfaces;
using NutritionPlanner.Core.Models;

namespace NutritionPlanner.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MealPlansController : ControllerBase
    {
        private readonly IMealPlanService _mealPlanService;
        private readonly IMealPlanItemService _mealPlanItemService;
        private readonly IUserProgressService _userProgressService;

        public MealPlansController(IMealPlanService mealPlanService, IMealPlanItemService mealPlanItemService, IUserProgressService userProgressService)
        {
            _mealPlanService = mealPlanService;
            _mealPlanItemService = mealPlanItemService;
            _userProgressService = userProgressService;
        }

        [HttpGet("{userId}/{date}")]
        public async Task<ActionResult<MealPlan>> GetMealPlanByUserIdAndDate(Guid userId, DateOnly date)
        {
            // Проверка существования плана питания на указанную дату
            var mealPlan = await _mealPlanService.GetMealPlanByUserIdAndDateAsync(userId, date);

            if (mealPlan == null)
            {
                // Создаём новый MealPlan, если его нет
                mealPlan = new MealPlan
                {
                    UserId = userId,
                    Date = date,
                    TotalCalories = 0,
                    TotalProtein = 0,
                    TotalFat = 0,
                    TotalCarbohydrates = 0,
                    MealPlanItems = new List<MealPlanItem>()
                };

                // Сохраняем новый план питания
                var newMealPlanId = await _mealPlanService.CreateMealPlanAsync(mealPlan);

                // Повторно получаем сохраненный план с реальным Id
                mealPlan = await _mealPlanService.GetMealPlanByUserIdAndDateAsync(userId, date);
            }

            var userProgress = new UserProgress
            {
                UserId = userId,
                Date = date, 
                CaloriesConsumed = mealPlan.TotalCalories,
                ProteinConsumed = mealPlan.TotalProtein,
                FatConsumed = mealPlan.TotalFat,
                CarbohydratesConsumed = mealPlan.TotalCarbohydrates,
                WaterConsumed = 0,
                ActivityMinutes = 0,
                Weight = 0,
            };

            // Добавляем или обновляем прогресс пользователя
            await _userProgressService.AddUserProgressAsync(userProgress);

            return Ok(mealPlan);
        }

        // Метод для получения отчета за текущую неделю (с понедельника по воскресенье)
        [HttpGet("weekly-report/current/{userId}")]
        public async Task<ActionResult<WeeklyReport>> GetCurrentWeeklyReport(Guid userId)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            int daysToSubtract = today.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)today.DayOfWeek - 1;
            var startOfWeek = today.AddDays(-daysToSubtract);
            var endOfWeek = startOfWeek.AddDays(6);

            var mealPlans = await _mealPlanService.GetMealPlansByUserIdAndDateRangeAsync(userId, startOfWeek, endOfWeek);

            var weeklyReport = new WeeklyReport
            {
                UserId = userId,
                StartDate = startOfWeek,
                EndDate = endOfWeek,
                MealPlans = mealPlans
            };

            return Ok(weeklyReport);
        }

        // Метод для получения отчета за предыдущую неделю (с понедельника по воскресенье)
        [HttpGet("weekly-report/previous/{userId}")]
        public async Task<ActionResult<WeeklyReport>> GetPreviousWeeklyReport(Guid userId)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            int daysToSubtractCurrent = today.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)today.DayOfWeek - 1;
            var startOfCurrentWeek = today.AddDays(-daysToSubtractCurrent);
            var startOfPreviousWeek = startOfCurrentWeek.AddDays(-7);
            var endOfPreviousWeek = startOfPreviousWeek.AddDays(6);

            var mealPlans = await _mealPlanService.GetMealPlansByUserIdAndDateRangeAsync(userId, startOfPreviousWeek, endOfPreviousWeek);

            var weeklyReport = new WeeklyReport
            {
                UserId = userId,
                StartDate = startOfPreviousWeek,
                EndDate = endOfPreviousWeek,
                MealPlans = mealPlans
            };

            return Ok(weeklyReport);
        }

        [HttpPost("{userId}/{date}")]
        public async Task<ActionResult<int>> CreateOrUpdateMealPlan(Guid userId, DateOnly date, [FromBody] MealPlan mealPlan)
        {
            // Проверка существования плана питания на указанную дату
            var existingPlan = await _mealPlanService.GetMealPlanByUserIdAndDateAsync(userId, date);
            if (existingPlan != null)
            {
                return Ok(existingPlan.Id); // Если план уже есть, возвращаем его ID
            }

            // Если плана нет, создаем новый
            var newMealPlan = new MealPlan
            {
                UserId = userId,
                Date = date,
                TotalCalories = 0,
                TotalProtein = 0,
                TotalFat = 0,
                TotalCarbohydrates = 0,
                MealPlanItems = mealPlan.MealPlanItems // Добавляем MealPlanItems
            };

            var mealPlanId = await _mealPlanService.CreateMealPlanAsync(newMealPlan);

            // Создание MealPlanItems, если они есть
            foreach (var item in newMealPlan.MealPlanItems)
            {
                item.MealPlanId = mealPlanId;
                await _mealPlanItemService.AddMealPlanItemAsync(item);
            }

            return CreatedAtAction(nameof(GetMealPlanByUserIdAndDate), new { userId = userId, date = date }, mealPlanId);
        }

    }
}