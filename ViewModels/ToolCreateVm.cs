using System.ComponentModel.DataAnnotations;

namespace pets.ViewModels
{
    public class ToolCreateVm
    {
        [Required(ErrorMessage = "اسم الأداة مطلوب")]
        [MaxLength(200)]
        public string Name { get; set; } = "";

        [MaxLength(1000)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
