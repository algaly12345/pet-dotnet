using System.ComponentModel.DataAnnotations;

namespace pets.ViewModels
{
    public class RoleFormVm
    {
        public string? Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = "";

        // القيم المختارة فقط
        public List<string> SelectedPermissions { get; set; } = new();

        // كل الصلاحيات للعرض في الفورم
        public List<RolePermissionVm> AllPermissions { get; set; } = new();
    }
}
