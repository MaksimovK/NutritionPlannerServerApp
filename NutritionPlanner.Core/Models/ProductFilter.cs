namespace NutritionPlanner.Core.Models
{
    public class ProductFilter
    {
        public bool? HighProtein { get; set; }
        public bool? LowCalorie { get; set; }
        public bool? HighCalorie { get; set; }
        public bool? LowCarb { get; set; }
        public bool? HighCarb { get; set; }
        public bool? LowFat { get; set; }
        public bool? HighFat { get; set; }
    }
}