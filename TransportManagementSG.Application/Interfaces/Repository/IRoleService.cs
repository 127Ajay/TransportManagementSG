using TransportManagementSG.Contracts.Model;

namespace TransportManagementSG.Application.Interfaces.Repository;

public interface IRoleService
{    
    Task<IEnumerable<Role>> GetAllRoles(CancellationToken token = default);
}