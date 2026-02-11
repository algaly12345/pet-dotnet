using System.ComponentModel.DataAnnotations;

namespace pets.Models
{
    public class Pet
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = "";

        [MaxLength(100)]
        public string? Type { get; set; }  // Dog, Cat...

        public int? OwnerId { get; set; }
        public Owner? Owner { get; set; }
    }
}
