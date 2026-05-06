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

        public async Task<List<User>> GetAllUsersAsync(CancellationToken token)
        {
            var users = await _userRepository.GetAllUsersAsync(token);

            return users.Select(u => new User
            {
                UserId = u.UserId,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Role    = u.Role   // if coming from SP
            }).ToList();
        }

        public async Task AddUser(User model, CancellationToken token = default)
        {           

            await _userRepository.AddUser(model, token);
        }

        public async Task DeleteUserAsync(int userId, CancellationToken token = default)
        {
            await _userRepository.DeleteUserAsync(userId, token);
        }

        public async Task<User> GetUserByIdAsync(int userId, CancellationToken token = default)
        {
            return await _userRepository.GetUserByIdAsync(userId, token);
        }

        public async Task UpdateUserAsync(User model, CancellationToken token = default)
        {
            await _userRepository.UpdateUserAsync(model, token);
        }
    }
}
