using TransportManagementSG.Contracts.Model;

namespace TransportManagementSG.Application.Interfaces.Repository;

public interface IRoleService
{
    Task<IEnumerable<Role>> GetAllRoles(CancellationToken cancellationToken = default);
    Task<Role?> GetRoleById(int id, CancellationToken cancellationToken = default);
    Task<Role> GetRoleByName(string roleName, CancellationToken cancellationToken = default);
    Task<int> CreateRole(Role role, CancellationToken cancellationToken = default);
    Task<bool> UpdateRole(Role role, CancellationToken cancellationToken = default);
    Task<bool> DeleteRole(int id, CancellationToken cancellationToken = default);
}