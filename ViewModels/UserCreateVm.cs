using System.ComponentModel.DataAnnotations;

namespace pets.ViewModels
{
    public class UserCreateVm
    {
        [Required, MaxLength(100)]
        public string UserName { get; set; } = "";

        [MaxLength(200)]
        public string? FullName { get; set; }

        public bool IsActive { get; set; } = true;

        // اختر Role واحد (يمكن تطويرها لاحقاً لـ multi roles)
        [Required]
        public string RoleName { get; set; } = "";

        // كلمة مرور
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "كلمتا المرور غير متطابقتين")]
        public string ConfirmPassword { get; set; } = "";
    }
}
