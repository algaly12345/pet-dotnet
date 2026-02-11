using System.ComponentModel.DataAnnotations;

namespace pets.Models
{
    public class Owner
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string FullName { get; set; } = "";

        [MaxLength(50)]
        public string? Phone { get; set; }

        [MaxLength(200)]
        public string? Email { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }
    }
}
