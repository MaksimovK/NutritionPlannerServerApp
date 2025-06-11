namespace NutritionPlanner.Application.Utilities
{
    public interface INutrition
    {
        decimal AdjustCaloriesForGoal(decimal calories, int goalTypeId);
        (decimal Protein, decimal Fat, decimal Carbohydrates) CalculateBJU(decimal totalCalories, decimal weight, int goalTypeId);
        decimal CalculateCalories(decimal weight, decimal height, int age, string gender, int activityLevel, int goalTypeId);
        decimal GetActivityCoefficient(int activityLevel);
    }
}