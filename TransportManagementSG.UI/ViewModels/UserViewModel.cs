using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TransportManagementSG.UI.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public int? RoleID { get; set; }
        public bool IsActive { get; set; }
        [ValidateNever]
        public List<SelectListItem> Roles { get; set; }
    }
}
