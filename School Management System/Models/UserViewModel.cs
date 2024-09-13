using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolManagementSystem.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace School_Management_System.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        // For selecting roles
        [Display(Name = "Role")]
        public string SelectedRole { get; set; }

        public string RoleId { get; set; }

        // List of roles to populate in dropdown
        public IEnumerable<SelectListItem> Roles { get; set; }

    }
}
