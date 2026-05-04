using TransportManagementSG.Application.Interfaces.Repository;
using TransportManagementSG.Contracts.Model;

namespace TransportManagementSG.Application.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }
    public async Task<IEnumerable<Role>> GetAllRoles(CancellationToken cancellationToken = default)
    {
        return await _roleRepository.GetAllRoles(cancellationToken);
    }

    public async Task<Role?> GetRoleById(int id, CancellationToken cancellationToken = default)
    {
        return await _roleRepository.GetRoleById(id, cancellationToken);
    }

    public async Task<Role> GetRoleByName(string roleName, CancellationToken cancellationToken = default)
    {
        return await _roleRepository.GetRoleByName(roleName, cancellationToken);
    }

    public async Task<int> CreateRole(Role role, CancellationToken cancellationToken = default)
    {
        return await _roleRepository.CreateRole(role, cancellationToken);
    }

    public async Task<bool> UpdateRole(Role role, CancellationToken cancellationToken = default)
    {
        return await _roleRepository.UpdateRole(role, cancellationToken);
    }

    public async Task<bool> DeleteRole(int id, CancellationToken cancellationToken = default)
    {
        return await _roleRepository.DeleteRole(id, cancellationToken);
    }
}