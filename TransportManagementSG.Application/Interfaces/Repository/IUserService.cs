using System;
using System.Collections.Generic;
using System.Text;
using TransportManagementSG.Contracts.Model;


namespace TransportManagementSG.Application.Interfaces.Repository
{
    public interface IUserService
    {
        Task<User> ValidateUserAsync(string email, string password, CancellationToken token = default);

        Task<List<User>> GetAllUsersAsync(CancellationToken token);
        Task AddUser(User model, CancellationToken token = default);
        Task DeleteUserAsync(int userId, CancellationToken token = default);
        Task<User> GetUserByIdAsync(int userId, CancellationToken token = default);

        Task UpdateUserAsync(User model, CancellationToken token = default);

    }
}
    