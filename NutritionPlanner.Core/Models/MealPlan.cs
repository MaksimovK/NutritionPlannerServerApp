namespace NutritionPlanner.Core.Models
{
    public class MealPlan
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public DateOnly Date { get; set; }
        public decimal TotalCalories { get; set; }
        public decimal TotalProtein { get; set; }
        public decimal TotalFat { get; set; }
        public decimal TotalCarbohydrates { get; set; }
        public List<MealPlanItem> MealPlanItems { get; set; } = new List<MealPlanItem>();
    }
}       
