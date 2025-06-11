using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutritionPlanner.DataAccess.Entities
{
    public class MealPlanEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public DateOnly Date { get; set; }
        public decimal TotalCalories { get; set; }
        public decimal TotalProtein { get; set; }
        public decimal TotalFat { get; set; }
        public decimal TotalCarbohydrates { get; set; }
        public ICollection<MealPlanItemEntity> MealPlanItems { get; set; } = new List<MealPlanItemEntity>();
    }
}
