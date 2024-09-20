using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    // Path to the file where users are stored
    private const string FilePath = "users.json";

    public UserFileRepository()
    {
        // Initialize the file if it doesn't exist
        if (!File.Exists(FilePath))
        {
            // Initialize with an empty JSON array
            File.WriteAllText(FilePath, "[]");  // Initialize with an empty JSON array
        }
    }

    // Add a new user to the repository
    public async Task<User> AddUserAsync(User user)
    {
        // Read the current list of users from the file
        var usersAsJson = await File.ReadAllTextAsync(FilePath);
        var users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;

        // Ensure users provide a user ID
        if (user.UserId == 0)
        {
            throw new InvalidOperationException("User ID must be provided.");
        }

        // Check if the user already exists
        var existingUser = users.SingleOrDefault(u => u.UserId == user.UserId);
        if (existingUser != null)
        {
            throw new InvalidOperationException($"User with ID '{user.UserId}' already exists.");
        }

        // Add the user to the list
        users.Add(user);

        // Serialize the updated list back to the file
        usersAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(FilePath, usersAsJson);

        return user;
    }

    // Update an existing user
    public async Task UpdateUserAsync(User user)
    {
        // Read the current list of users from the file
        var usersAsJson = await File.ReadAllTextAsync(FilePath);
        var users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;

        // Find the existing user by ID
        var oldUser = users.SingleOrDefault(u => u.UserId == user.UserId);
        if (oldUser == null)
        {
            throw new InvalidOperationException($"User with ID '{user.UserId}' not found");
        }

        // Update the user properties
        oldUser.UserName = user.UserName;
        oldUser.Password = user.Password;

        // Serialize the updated list back to the file
        usersAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(FilePath, usersAsJson);
    }

    // Delete a user by ID
    public async Task DeleteUserAsync(int userId)
    {
        // Read the current list of users from the file
        var usersAsJson = await File.ReadAllTextAsync(FilePath);
        var users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;

        // Find the user to remove by ID
        var userToRemove = users.SingleOrDefault(u => u.UserId == userId);
        if (userToRemove == null)
        {
            throw new InvalidOperationException($"User with ID '{userId}' not found");
        }

        // Remove the user from the list
        users.Remove(userToRemove);

        // Serialize the updated list back to the file
        usersAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(FilePath, usersAsJson);
    }

    // Get a single user by ID
    public async Task<User> GetSingleUserAsync(int id)
    {
        // Read the current list of users from the file
        var usersAsJson = await File.ReadAllTextAsync(FilePath);
        var users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;

        // Find the user by ID
        var user = users.SingleOrDefault(u => u.UserId == id);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID '{id}' not found");
        }

        return user;
    }

    // Get many users (as IQueryable)
    public IQueryable<User> GetUserMany()
    {
        // Read the current list of users from the file
        var usersAsJson = File.ReadAllTextAsync(FilePath).Result;
        var users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;

        // Return the list of users as a queryable collection
        return users.AsQueryable();
    }
}
