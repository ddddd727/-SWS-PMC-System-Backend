using System.ComponentModel.DataAnnotations;

namespace PMCSystem_Backend.Entities
{
    public class ExampleEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Message { get; set; } = string.Empty;

        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }
}
