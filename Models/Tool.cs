using System.ComponentModel.DataAnnotations;

namespace pets.Models
{
    public class Tool
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = "";

        [MaxLength(1000)]
        public string? Description { get; set; }   // ✅ موجودة

        public bool IsActive { get; set; } = true;
    }
}
