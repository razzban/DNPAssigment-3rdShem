using RepositoryContracts;

namespace CLI.UI.SpecificSearches;

public class SpecificSearch(IUserRepository userRepo, ICommentRepository commentRepo, IPostRepository postRepo)
{
    public async Task FilterSearchAsync()
    {
        Console.WriteLine("1. Search all posts by User ID");
        Console.WriteLine("2. Search all comments by User ID");
        Console.WriteLine("3. Search all users with specific name in username");
        Console.WriteLine("4. Exit");
        var choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                await SearchPostsByUserIdAsync();
                break;
            case "2":
                await SearchCommentsByUserIdAsync();
                break;
            case "3":
                await SearchUsersByNameAsync();
                break;
            case "4":
                return;
            default:
                Console.WriteLine("Wrong choice");
                break;
        }
    }

    private async Task SearchPostsByUserIdAsync()
    {
        Console.WriteLine("Enter user ID:");
        var userId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException("Invalid User ID"));
        var posts = postRepo.GetPostMany().Where(p => p.UserId == userId);
        foreach (var post in posts)
        {
            Console.WriteLine($"Post ID: {post.PostId}, Title: {post.Title}, Body: {post.Body}");
        }
        await Task.CompletedTask;
    }

    private async Task SearchCommentsByUserIdAsync()
    {
        Console.WriteLine("Enter user ID:");
        var userId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException("Invalid User ID"));
        var comments = commentRepo.GetCommentMany().Where(c => c.UserId == userId);
        foreach (var comment in comments)
        {
            Console.WriteLine($"Comment ID: {comment.CommentId}, Post ID: {comment.PostId}, Body: {comment.Body}");
        }
        await Task.CompletedTask;
    }

    private async Task SearchUsersByNameAsync()
    {
        Console.WriteLine("Enter Keyword:");
        var keyword = Console.ReadLine();
        var users = userRepo.GetUserMany().Where(u => u.UserName != null && keyword != null && u.UserName.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
        if (users.Count != 0)
        {
            foreach (var user in users)
            {
                Console.WriteLine($"User ID: {user.UserId}, Name: {user.UserName}");
            }
        }
        else
        {
            Console.WriteLine("No users found with that keyword.");
        }
        await Task.CompletedTask;
    }
}