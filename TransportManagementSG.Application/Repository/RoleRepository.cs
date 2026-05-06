using Dapper;
using System.Data;
using TransportManagementSG.Application.Database;
using TransportManagementSG.Application.Interfaces.Repository;
using TransportManagementSG.Contracts.Model;

namespace TransportManagementSG.Application.Repository;

public class RoleRepository : IRoleRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public RoleRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IEnumerable<Role>> GetAllRoles(CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        //using var transaction = connection.BeginTransaction();

        var roles = await connection.QueryAsync<Role>(
            new CommandDefinition("usp_GetAllRoles", cancellationToken: cancellationToken));

        return roles;
    }

    public async Task<Role?> GetRoleById(int id, CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        return await connection.QueryFirstOrDefaultAsync<Role>(
            new CommandDefinition(
                "usp_GetRoleById",
                new { RoleId = id },
                commandType: System.Data.CommandType.StoredProcedure,
                cancellationToken: cancellationToken));
    }

    public async Task<Role?> GetRoleByName(string roleName, CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        return await connection.QueryFirstOrDefaultAsync<Role>(
            new CommandDefinition(
                "usp_GetRolesByName",
                new { RoleName = roleName },
                commandType: System.Data.CommandType.StoredProcedure,
                cancellationToken: cancellationToken));
    }

    public async Task<int> CreateRole(Role role, CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        return await connection.QuerySingleAsync<int>(
            new CommandDefinition(
                "usp_CreateRole",
                new { role.RoleName, role.IsActive },
                commandType: System.Data.CommandType.StoredProcedure,
                cancellationToken: cancellationToken));
    }

    public async Task<bool> UpdateRole(Role role, CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var rows = await connection.ExecuteAsync(
            new CommandDefinition(
                "usp_UpdateRole",
                new { role.RoleId, role.RoleName, role.IsActive },
                commandType: System.Data.CommandType.StoredProcedure,
                cancellationToken: cancellationToken));

        return rows > 0;
    }

    public async Task<bool> DeleteRole(int id, CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var rows = await connection.ExecuteAsync(
            new CommandDefinition(
                "usp_DeleteRole",
                new { RoleId = id },
                commandType: System.Data.CommandType.StoredProcedure,
                cancellationToken: cancellationToken));

        return rows > 0;
    }
}