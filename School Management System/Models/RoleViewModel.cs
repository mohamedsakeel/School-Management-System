using System.ComponentModel.DataAnnotations;

namespace School_Management_System.Models
{
    public class RoleViewModel
    {
        public string RoleId { get; set; }

        [Required(ErrorMessage = "Role name is required")]
        [StringLength(256, ErrorMessage = "Role name cannot be longer than 256 characters")]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}
