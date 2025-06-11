using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutritionPlanner.DataAccess.Entities
{
    public class MealPlanItemEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int MealPlanId { get; set; }
        public int MealTimeId { get; set; }
        public int? ProductId { get; set; }
        public int? RecipeId { get; set; }
        public decimal Amount { get; set; }
    }
}
