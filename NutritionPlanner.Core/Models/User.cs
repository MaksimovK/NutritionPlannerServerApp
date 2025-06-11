namespace NutritionPlanner.Core.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public int ActivityLevelId { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public Role Role { get; set; }
    }
}
