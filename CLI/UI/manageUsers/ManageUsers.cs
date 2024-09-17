using Entities;
using RepositoryContracts;

namespace CLI.UI.manageUsers;

public class ManageUsers(IUserRepository userRepo)
{
    public async Task ManageUsersAsync()
    {
        Console.WriteLine("1. Create User");
        Console.WriteLine("2. View Users");
        Console.WriteLine("3. Update User");
        Console.WriteLine("4. Delete User");
        Console.WriteLine("5. Exit");
        var choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                await CreateUserAsync();
                break;
            case "2":
                ViewUsers();
                break;
            case "3":
                await UpdateUserAsync();
                break;
            case "4":
                await DeleteUserAsync();
                break;
            case "5":
                return;
            default:
                Console.WriteLine("Wrong choice");
                break;
        }
    }

    private async Task CreateUserAsync()
    {
        Console.WriteLine("Enter user ID:");
        var id = int.Parse(Console.ReadLine() ?? string.Empty);
        Console.WriteLine("Enter username:");
        var username = Console.ReadLine();
        Console.WriteLine("Enter password:");
        var password = Console.ReadLine();

        var newUser = new User { UserId = id, UserName = username, Password = password };

        try
        {
            await userRepo.AddUserAsync(newUser);
            Console.WriteLine("User created successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void ViewUsers()
    {
        var users = userRepo.GetUserMany();
        foreach (var user in users)
        {
            Console.WriteLine($"ID: {user.UserId}, Name: {user.UserName}");
        }
    }

    private async Task UpdateUserAsync()
    {
        Console.WriteLine("Enter user ID to Update:");
        var userId = int.Parse(Console.ReadLine() ?? string.Empty);

        var user = await userRepo.GetSingleUserAsync(userId);

        Console.WriteLine("Enter new username:");
        var username = Console.ReadLine();
        Console.WriteLine("Enter new password:");
        var password = Console.ReadLine();

        user.UserName = username;
        user.Password = password;

        await userRepo.UpdateUserAsync(user);

        Console.WriteLine("User updated successfully.");
    }

    private async Task DeleteUserAsync()
    {
        Console.WriteLine("Enter user ID to Delete:");
        var userId = int.Parse(Console.ReadLine() ?? string.Empty);

        await userRepo.DeleteUserAsync(userId);

        Console.WriteLine("User deleted successfully.");
    }
}