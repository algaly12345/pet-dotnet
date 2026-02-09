using System.ComponentModel.DataAnnotations;

namespace pets.Models
{
    public class ToolItem
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; } = "";

        [StringLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
