using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ManagePosts(IPostRepository postRepo)
{
    public async Task ManagePostsAsync()
    {
        Console.WriteLine("1. Create Post");
        Console.WriteLine("2. View Posts");
        Console.WriteLine("3. Update Post");
        Console.WriteLine("4. Delete Post");
        Console.WriteLine("5. Exit");
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
                await UpdatePostAsync();
                break;
            case "4":
                await DeletePostAsync();
                break;
            case "5":
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
        await postRepo.AddPostAsync(newPost);

        Console.WriteLine("Post created successfully.");
    }

    private void ViewPosts()
    {
        // Get all posts
        var posts = postRepo.GetPostMany();
        // Print each post and its comments
        foreach (var post in posts)
        {
            // Print post details
            Console.WriteLine($"Post ID: {post.PostId}, Title: {post.Title}, Body: {post.Body}");
       // Print comments if any
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

    private async Task UpdatePostAsync()
    {
        Console.WriteLine("Enter post ID to Update:");
        var postId = int.Parse(Console.ReadLine() ?? string.Empty);

        var post = await postRepo.GetSinglePostAsync(postId);

        Console.WriteLine("Enter new post title:");
        var title = Console.ReadLine();
        Console.WriteLine("Enter new post body:");
        var body = Console.ReadLine();

        post.Title = title;
        post.Body = body;
        await postRepo.UpdatePostAsync(post);

        Console.WriteLine("Post updated successfully.");
    }

    private async Task DeletePostAsync()
    {
        Console.WriteLine("Enter post ID to Delete:");
        var postId = int.Parse(Console.ReadLine() ?? string.Empty);

        await postRepo.DeletePostAsync(postId);

        Console.WriteLine("Post deleted successfully.");
    }
}