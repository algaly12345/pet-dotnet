using System.ComponentModel.DataAnnotations;

namespace pets.ViewModels
{
    public class LoginVm
    {
        [Required]
        public string UserName { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        public bool RememberMe { get; set; }
    }
}
