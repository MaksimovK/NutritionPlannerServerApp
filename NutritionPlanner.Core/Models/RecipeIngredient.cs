namespace NutritionPlanner.Core.Models
{
    public class RecipeIngredient
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int ProductId { get; set; }
        public decimal Amount { get; set; }
        public Recipe Recipe { get; set; }
        public Product Product { get; set; }
    }
}
