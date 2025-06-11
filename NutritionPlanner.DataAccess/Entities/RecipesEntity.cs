using NutritionPlanner.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutritionPlanner.DataAccess.Entities
{
    public class RecipeEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsApproved { get; set; } = true;
        public Guid? CreatedByUserId { get; set; }

        [NotMapped] // Не сохранять в БД, вычисляемое поле
        public decimal TotalWeight => Ingredients?.Sum(i => i.Amount) ?? 0;

        public virtual ICollection<RecipeIngredientEntity> Ingredients { get; set; }
    }
}
