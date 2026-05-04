using TransportManagementSG.Contracts.Model;
using TransportManagementSG.UI.ViewModels;

namespace TransportManagementSG.UI.Extensions;

public static class RoleMappingExtensions
{
    public static RoleViewModel ToViewModel(this Role role)
    {
        return new RoleViewModel
        {
            RoleId = role.RoleId,
            RoleName = role.RoleName,
            IsActive = role.IsActive
        };
    }

    public static Role ToModel(this RoleViewModel model)
    {
        return new Role
        {
            RoleId = model.RoleId,
            RoleName = model.RoleName,
            IsActive = model.IsActive
        };
    }
}
