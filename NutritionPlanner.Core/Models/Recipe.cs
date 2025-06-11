namespace NutritionPlanner.Core.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<RecipeIngredient> Ingredients { get; set; }
        public bool IsApproved { get; set; }
        public Guid? CreatedByUserId { get; set; }
    }
}
