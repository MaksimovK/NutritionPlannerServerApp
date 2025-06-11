namespace NutritionPlanner.Core.Models
{
    public class UserProgress
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public DateOnly Date { get; set; }
        public decimal? Weight { get; set; }
        public decimal CaloriesConsumed { get; set; }
        public decimal ProteinConsumed { get; set; }
        public decimal FatConsumed { get; set; }
        public decimal CarbohydratesConsumed { get; set; }
        public decimal? WaterConsumed { get; set; }
        public int? ActivityMinutes { get; set; }

    }
}
