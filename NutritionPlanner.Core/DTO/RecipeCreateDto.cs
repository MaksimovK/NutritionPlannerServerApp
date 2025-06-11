namespace NutritionPlanner.Core.DTO
{
    namespace NutritionPlanner.Core.Models
    {
        public class RecipeCreateDto
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public List<RecipeIngredientDto> Ingredients { get; set; }
        }

        public class RecipeWithNutritionDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal TotalWeight { get; set; } 
            public decimal CaloriesPer100g { get; set; }
            public decimal ProteinPer100g { get; set; }
            public decimal FatPer100g { get; set; }
            public decimal CarbohydratesPer100g { get; set; }
            public List<RecipeIngredientDto> Ingredients { get; set; }
        }

        public class RecipeIngredientDto
        {
            public int Id { get; set; }
            public int RecipeId { get; set; }
            public int ProductId { get; set; }
            public decimal Amount { get; set; }
            public ProductDto Product { get; set; }
        }

        public class ProductDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Calories { get; set; }         // ккал на 100 г
            public decimal Protein { get; set; }          // белки на 100 г
            public decimal Fat { get; set; }              // жиры на 100 г
            public decimal Carbohydrates { get; set; }    // углеводы на 100 г
        }
    }
}
