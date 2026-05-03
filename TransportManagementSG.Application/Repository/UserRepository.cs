using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TransportManagementSG.Application.Database;
using TransportManagementSG.Application.Interfaces.Repository;
using TransportManagementSG.Contracts.Model;

namespace TransportManagementSG.Application.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public UserRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<User> ValidateUserAsync(string email, string password, CancellationToken token = default)
        {
            using var db = await _dbConnectionFactory.CreateConnectionAsync(token);

            var parameters = new DynamicParameters();
            parameters.Add("@Email", email);
            parameters.Add("@Password", password);

            return await db.QueryFirstOrDefaultAsync<User>(
                "sp_ValidateUser",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
