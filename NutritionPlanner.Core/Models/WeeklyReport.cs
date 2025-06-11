namespace NutritionPlanner.Core.Models
{
    public class WeeklyReport
    {
        public Guid UserId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public List<MealPlan> MealPlans { get; set; } = new List<MealPlan>();
    }
}
