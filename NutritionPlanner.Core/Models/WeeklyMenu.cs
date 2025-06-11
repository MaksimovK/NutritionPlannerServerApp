namespace NutritionPlanner.Core.Models
{
    public class WeeklyMenu
    {
        public int Id { get; set; }
        public int GoalTypeId { get; set; }
        public string DayOfWeek { get; set; }
        public int MealTimeId { get; set; }
        public int? RecipeId { get; set; }
        public int? ProductId { get; set; }
        public decimal Amount { get; set; }
    }
}
