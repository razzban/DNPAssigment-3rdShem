
using Entities;
using RepositoryContracts;

namespace InMemoryRepositories
{
    public class UserInMemoryRepository : IUserRepository
    {
        //list of users
        private readonly List<User> _users = [];

        public Task<User> AddUserAsync(User user)
        {
            //Ensure users provide a user ID
            if (user.UserId == 0)
            {
                throw new InvalidOperationException("User ID must be provided.");
            }
            
            // Check if the user already exists
            var existingUser = _users.SingleOrDefault(u => u.UserId == user.UserId);
            if (existingUser != null)
            {
                throw new InvalidOperationException($"User with ID '{user.UserId}' already exists.");
            }
            
            // Set the user ID that user provides
            _users.Add(user);
            return Task.FromResult(user);
        }

        public Task UpdateUserAsync(User user)
        {
            // Find the existing user by ID
            var oldUser = _users.SingleOrDefault(u => u.UserId == user.UserId);
            // If the user does not exist, throw an exception
            if (oldUser == null)
            {
                throw new InvalidOperationException($"User with ID '{user.UserId}' not found");
            }
            
            // Update the user properties
            oldUser.UserName = user.UserName;
            oldUser.Password = user.Password;

            
            return Task.CompletedTask;
        }

        public Task DeleteUserAsync(int userId)
        {
            // Find the user to remove by ID
            var userToRemove = _users.SingleOrDefault(u => u.UserId == userId);
            // If the user does not exist, throw an exception
            if (userToRemove == null)
            {
                throw new InvalidOperationException($"User with ID '{userId}' not found");
            }
            
            // Remove the user from the list
            _users.Remove(userToRemove);
            return Task.CompletedTask;
        }

        public Task<User> GetSingleUserAsync(int id)
        {
            // Find the user by ID and return it
            var user = _users.SingleOrDefault(u => u.UserId == id);
            // If the user does not exist, throw an exception
            if (user == null)
            {
                throw new InvalidOperationException($"User with ID '{id}' not found");
            }
            // Return the user
            return Task.FromResult(user);
        }

        public IQueryable<User> GetUserMany()
        {
            // Return the list of users as a queryable collection
            return _users.AsQueryable();
        }
    }
}