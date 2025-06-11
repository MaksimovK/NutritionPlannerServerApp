namespace NutritionPlanner.Core.Models
{
    public class MealPlanItem
    {
        public int Id { get; set; }
        public int MealPlanId { get; set; }
        public int MealTimeId { get; set; }
        public int? ProductId { get; set; }
        public int? RecipeId { get; set; }
        public decimal Amount { get; set; }
    }
}
