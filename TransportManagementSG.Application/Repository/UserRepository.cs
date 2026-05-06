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

        public async Task<List<User>> GetAllUsersAsync(CancellationToken token = default)
        {
            using var db = await _dbConnectionFactory.CreateConnectionAsync(token);
            var result = await db.QueryAsync<User>(
                   "sp_GetAllUsers",
                   commandType: CommandType.StoredProcedure
               );

            return result.ToList();
        }

        public async Task AddUser(User model, CancellationToken token = default)
        {
            using var db = await _dbConnectionFactory.CreateConnectionAsync(token);

            var parameters = new DynamicParameters();
            parameters.Add("@FirstName", model.FirstName);
            parameters.Add("@LastName", model.LastName);
            parameters.Add("@Email", model.Email);
            parameters.Add("@PhoneNumber", model.PhoneNumber);
            parameters.Add("@RoleID", model.RoleID);

            await db.ExecuteAsync(
                "usp_CreateUser",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task DeleteUserAsync(int userId, CancellationToken token = default)
        {
            using var db = await _dbConnectionFactory.CreateConnectionAsync(token);

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);

            await db.ExecuteAsync(
                "usp_DeleteUser",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<User> GetUserByIdAsync(int userId, CancellationToken token = default)
        {
            using var db = await _dbConnectionFactory.CreateConnectionAsync(token);

            var param = new DynamicParameters();
            param.Add("@UserId", userId);

            return await db.QueryFirstOrDefaultAsync<User>(
                "usp_GetUserById",
                param,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task UpdateUserAsync(User model, CancellationToken token = default)
        {
            using var db = await _dbConnectionFactory.CreateConnectionAsync(token);

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", model.UserId);
            parameters.Add("@FirstName", model.FirstName);
            parameters.Add("@LastName", model.LastName);
            parameters.Add("@Email", model.Email);
            parameters.Add("@PhoneNumber", model.PhoneNumber);
            parameters.Add("@RoleID", model.RoleID);

            await db.ExecuteAsync(
                "usp_UpdateUser",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

    }
}
