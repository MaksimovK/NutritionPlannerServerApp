using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutritionPlanner.DataAccess.Entities
{
    public class ProductEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public decimal Weight { get; set; } = 100;

        public decimal Calories { get; set; }
        public decimal Protein { get; set; }
        public decimal Fat { get; set; }
        public decimal Carbohydrates { get; set; }

        [StringLength(20)]
        public string? Barcode { get; set; }
        public bool IsApproved { get; set; } = true; // По умолчанию true для админов
        public Guid? CreatedByUserId { get; set; } // Ссылка на создателя
    }
}
