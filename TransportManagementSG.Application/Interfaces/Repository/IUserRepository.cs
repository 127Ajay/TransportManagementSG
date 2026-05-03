using System;
using System.Collections.Generic;
using System.Text;
using TransportManagementSG.Contracts.Model;

namespace TransportManagementSG.Application.Interfaces.Repository
{
    public interface IUserRepository    {
        Task<User> ValidateUserAsync(string email, string password, CancellationToken token = default);
    }
}
