using System.ComponentModel.DataAnnotations;

namespace pets.ViewModels
{
    public class UserEditVm
    {
        [Required]
        public string Id { get; set; } = "";

        [Required, MaxLength(100)]
        public string UserName { get; set; } = "";

        [MaxLength(200)]
        public string? FullName { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public string RoleName { get; set; } = "";

        // تغيير كلمة المرور (اختياري)
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "كلمتا المرور غير متطابقتين")]
        public string? ConfirmNewPassword { get; set; }
    }
}
