namespace NutritionPlanner.Application.Utilities
{
    public class Nutrition : INutrition
    {
        public decimal CalculateCalories(decimal weight, decimal height, int age, string gender, int activityLevel, int goalTypeId)
        {
            decimal bmr = gender == "male"
                ? (10 * weight) + (6.25m * height) - (5 * age) + 5
                : (10 * weight) + (6.25m * height) - (5 * age) - 161;

            decimal calories = bmr * GetActivityCoefficient(activityLevel);

            calories = AdjustCaloriesForGoal(calories, goalTypeId);

            return calories;
        }
        public decimal AdjustCaloriesForGoal(decimal calories, int goalTypeId)
        {
            switch (goalTypeId)
            {
                case 1: // Похудение
                    calories *= 0.9m; // Уменьшаем на 10%
                    break;

                case 2: // Набор массы
                    calories *= 1.1m; // Увеличиваем на 10%
                    break;

                case 3: // Поддержание веса
                default:
                    break;
            }

            return calories;
        }

        public decimal GetActivityCoefficient(int activityLevel)
        {
            return activityLevel switch
            {
                1 => 1.2m, // Сидячий образ жизни
                2 => 1.375m, // Маленькая активность
                3 => 1.55m, // Умеренная активность
                4 => 1.725m, // Высокая активность
                5 => 1.9m, // Экстремальная активность
                _ => 1.2m // Default (если уровень активности не найден)
            };
        }

        public (decimal Protein, decimal Fat, decimal Carbohydrates) CalculateBJU(decimal totalCalories, decimal weight, int goalTypeId)
        {
            if (totalCalories <= 0)
                throw new ArgumentException("Общее количество калорий должно быть больше 0.");
            if (weight <= 0)
                throw new ArgumentException("Масса тела должна быть больше 0.");

            decimal proteinPerKg, fatPerKg;

            switch (goalTypeId)
            {
                case 1: // Похудение
                    proteinPerKg = 1.8m;
                    fatPerKg = 0.8m;
                    break;
                case 2: // Набор массы
                    proteinPerKg = 2.0m;
                    fatPerKg = 1.3m;
                    break;
                case 3: // Поддержание веса
                default:
                    proteinPerKg = 1.6m;
                    fatPerKg = 0.9m;
                    break;
            }

            decimal protein = weight * proteinPerKg;
            decimal fat = weight * fatPerKg;

            decimal caloriesFromProtein = protein * 4;
            decimal caloriesFromFat = fat * 9;

            if (caloriesFromProtein + caloriesFromFat > totalCalories)
            {
                throw new InvalidOperationException("Калории от белков и жиров превышают общую калорийность.");
            }

            decimal remainingCalories = totalCalories - caloriesFromProtein - caloriesFromFat;

            decimal carbohydrates = remainingCalories / 4;

            return (Math.Round(protein, 2), Math.Round(fat, 2), Math.Round(carbohydrates, 2));
        }
    }
}
