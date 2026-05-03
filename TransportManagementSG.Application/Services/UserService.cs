using System;
using System.Collections.Generic;
using System.Text;
using TransportManagementSG.Application.Interfaces.Repository;
using TransportManagementSG.Contracts.Model;

namespace TransportManagementSG.Application.Services
{
    public class UserService : IUserService   
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> ValidateUserAsync(string email, string password, CancellationToken token = default)
        {
            return await _userRepository.ValidateUserAsync(email, password, token);
        }
    }
}
