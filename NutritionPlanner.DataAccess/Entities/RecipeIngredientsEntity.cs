using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutritionPlanner.DataAccess.Entities
{
    public class RecipeIngredientEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int RecipeId { get; set; }
        public int ProductId { get; set; }

        public decimal Amount { get; set; }

        public virtual RecipeEntity Recipe { get; set; }
        public virtual ProductEntity Product { get; set; }
    }
}
