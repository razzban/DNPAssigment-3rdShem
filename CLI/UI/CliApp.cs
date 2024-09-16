namespace CLI.UI;
using System;
using System.Threading.Tasks;
using RepositoryContracts;
using Entities;

public class CliApp(IUserRepository userRepo, IPostRepository postRepo, ICommentRepository commentRepo)
{
    public async Task StartAsync()
    {
        Console.WriteLine("Welcome to the CLI App!");
        while (true)
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Manage Users");
            Console.WriteLine("2. Manage Posts");
            Console.WriteLine("3. Add Comment");
            Console.WriteLine("4. Exit");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await ManageUsersAsync();
                    break;
                case "2":
                    await ManagePostsAsync();
                    break;
                case "3":
                    await AddCommentAsync();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid choice, please try again.");
                    break;
            }
        }
    }

    private async Task ManageUsersAsync()
    {
        Console.WriteLine("1. Create User");
        Console.WriteLine("2. View Users");
        Console.WriteLine("3. Exit");
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

        var newUser = new User { Id= id ,UserName = username, Password = password };
        await userRepo.AddAsync(newUser);

        Console.WriteLine("User created successfully.");
    }

    private void ViewUsers()
    {
        var users = userRepo.GetMany();
        foreach (var user in users)
        {
            Console.WriteLine($"ID: {user.Id}, Name: {user.UserName}");
        }
    }

    private async Task ManagePostsAsync()
    {
        Console.WriteLine("1. Create Post");
        Console.WriteLine("2. View Posts");
        var choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                await CreatePostAsync();
                break;
            case "2":
                ViewPosts();
                break;
            case "3":
                return;
            default:
                Console.WriteLine("Wrong choice");
                break;
        }
    }

    private async Task CreatePostAsync()
    {
        Console.WriteLine("Enter post title:");
        var title = Console.ReadLine();
        Console.WriteLine("Enter post body:");
        var body = Console.ReadLine();
        Console.WriteLine("Enter user ID:");
        var userId = int.Parse(Console.ReadLine() ?? string.Empty);

        var newPost = new Post { Title = title, Body = body, UserId = userId };
        await postRepo.AddAsync(newPost);

        Console.WriteLine("Post created successfully.");
    }

    private void ViewPosts()
    {
        var posts = postRepo.GetMany();

        foreach (var post in posts)
        {
            Console.WriteLine($"Post ID: {post.Id}, Title: {post.Title}");

            // Display comments if any exist
            if (post.Comments.Count != 0)
            {
                Console.WriteLine("  Comments:");
                foreach (var comment in post.Comments)
                {
                    Console.WriteLine($"    Comment by User {comment.UserId}: {comment.Body}");
                }
            }
            else
            {
                Console.WriteLine("  No comments yet.");
            }

            Console.WriteLine();
        }
    }



    private async Task AddCommentAsync()
    {
        Console.WriteLine("Enter post ID:");
        var postId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException("Invalid Post ID"));
        var post = await postRepo.GetSingleAsync(postId);
        Console.WriteLine("Enter user ID:");
        var userId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException("Invalid User ID"));
        Console.WriteLine("Enter comment body:");
        var body = Console.ReadLine();

        // Create a new comment
        var newComment = new Comment { PostId = postId, UserId = userId, Body = body };
        await commentRepo.AddAsync(newComment);
        post.Comments.Add(newComment);
        await postRepo.UpdateAsync(post);
        Console.WriteLine("Comment added successfully.");
    }

}

