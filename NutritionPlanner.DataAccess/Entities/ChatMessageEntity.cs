using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NutritionPlanner.DataAccess.Entities
{
    public class ChatMessageEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid SenderId { get; set; }

        [Required]
        public Guid ReceiverId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;

        // Навигационные свойства
        [ForeignKey("SenderId")]
        public virtual UserEntity Sender { get; set; }

        [ForeignKey("ReceiverId")]
        public virtual UserEntity Receiver { get; set; }

        [StringLength(24)] // IV всегда 16 байт -> base64 = 24 символа
        public string IV { get; set; } // Вектор инициализации
    }
}
