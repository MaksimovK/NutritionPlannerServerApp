using NutritionPlanner.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutritionPlanner.DataAccess.Entities
{
    public class UserEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public int Age { get; set; }
        public string Gender { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }

        public int ActivityLevelId { get; set; }

        public string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; }

        [Required]
        public Role Role { get; set; } = Role.User;
        public bool IsBlocked { get; set; } = false;
        public DateTime? BlockedUntil { get; set; } = null;
        public string? BlockReason { get; set; } = string.Empty;
    }
}
