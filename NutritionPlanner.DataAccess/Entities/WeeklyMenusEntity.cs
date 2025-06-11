using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutritionPlanner.DataAccess.Entities
{
    public class WeeklyMenuEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int GoalTypeId { get; set; }
        public string DayOfWeek { get; set; }
        public int MealTimeId { get; set; }
        public int? RecipeId { get; set; }
        public int? ProductId { get; set; }
        public decimal Amount { get; set; }
    }
}
