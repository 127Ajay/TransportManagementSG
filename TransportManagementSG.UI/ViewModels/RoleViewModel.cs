using System.ComponentModel.DataAnnotations;

namespace TransportManagementSG.UI.ViewModels;

public class RoleViewModel
{
    public int RoleId { get; set; }

    [Required(ErrorMessage = "Role Name is required")]
    [StringLength(50, ErrorMessage = "Max 50 characters")]
    public string RoleName { get; set; }
    public bool IsActive { get; set; }
}

