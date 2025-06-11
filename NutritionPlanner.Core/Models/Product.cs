namespace NutritionPlanner.Core.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Weight { get; set; } = 100;
        public decimal Calories { get; set; }
        public decimal Protein { get; set; }
        public decimal Fat { get; set; }
        public decimal Carbohydrates { get; set; }
        public string? Barcode { get; set; }
        public bool IsApproved { get; set; }
        public Guid? CreatedByUserId { get; set; }
    }
}
