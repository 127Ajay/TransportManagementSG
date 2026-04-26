using TransportManagementSG.Contracts.Model;

namespace TransportManagementSG.Application.Interfaces.Repository;

public interface IRoleService
{
    Task<IEnumerable<Role>> GetAllRoles(CancellationToken cancellationToken = default);
    Task<Role> GetRoleByName(string roleName, CancellationToken cancellationToken = default);
}