using Dapper;
using TransportManagementSG.Application.Database;
using TransportManagementSG.Application.Interfaces.Repository;
using TransportManagementSG.Contracts.Model;

namespace TransportManagementSG.Application.Repository;

public class RoleRepository: IRoleRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public RoleRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IEnumerable<Role>> GetAllRoles(CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        var roles = await connection.QueryAsync<Role>(
            new CommandDefinition("usp_GetAllRoles", cancellationToken));
        
        return roles;
    }

    public async Task<Role> GetRoleByName(string roleName, CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();
        
        var parameters = new DynamicParameters();
        parameters.Add("@RoleName", roleName);

        var role = await connection.QueryFirstOrDefaultAsync<Role>(
            new CommandDefinition("usp_GetAllRoles", parameters, cancellationToken: cancellationToken));
        
        return role;
    }
}