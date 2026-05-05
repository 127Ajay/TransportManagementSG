using Dapper;
using System.Data;
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

    public async Task<IEnumerable<Role>> GetAllRoles(CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        //using var transaction = connection.BeginTransaction();

        var roles = await connection.QueryAsync<Role>(
            new CommandDefinition("usp_GetAllRoles", cancellationToken: cancellationToken));
        
        return roles;
    }  

   
}