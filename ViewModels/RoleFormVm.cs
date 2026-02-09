using System.ComponentModel.DataAnnotations;

namespace pets.ViewModels
{
    public class RoleFormVm
    {
        public string? Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = "";

        // checkbox values from form
        public List<string> SelectedPermissions { get; set; } = new();

        // for displaying all permissions
        public List<RolePermissionVm> AllPermissions { get; set; } = new();
    }
}
