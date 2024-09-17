using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class ManageComments( IPostRepository postRepo, ICommentRepository commentRepo)
{
    

    public async Task ManageCommentAsync()
    {
        Console.WriteLine("1. Add Comment");
        Console.WriteLine("2. Update Comment");
        Console.WriteLine("3. Delete Comment");
        Console.WriteLine("4. Exit");
        var choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                await AddCommentAsync();
                break;
            case "2":
                await UpdateCommentAsync();
                break;
            case "3":
                await DeleteCommentAsync();
                break;
            case "4":
                return;
            default:
                Console.WriteLine("Wrong choice");
                break;
        }
    }

    private async Task AddCommentAsync()
    {
        Console.WriteLine("Enter post ID:");
        var postId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException("Invalid Post ID"));
        var post = await postRepo.GetSinglePostAsync(postId);

        Console.WriteLine("Enter user ID:");
        var userId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException("Invalid User ID"));

        Console.WriteLine("Enter comment body:");
        var body = Console.ReadLine();

        var newComment = new Comment { PostId = postId, UserId = userId, Body = body };
        await commentRepo.AddCommentAsync(newComment);

        post.Comments.Add(newComment);
        await postRepo.UpdatePostAsync(post);

        Console.WriteLine("Comment added successfully.");
    }

    private async Task UpdateCommentAsync()
    {
        Console.WriteLine("Enter comment ID:");
        var commentId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException("Invalid Comment ID"));

        Console.WriteLine("Enter your user ID:");
        var userId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException("Invalid User ID"));

        var comment = await commentRepo.GetSingleCommentAsync(commentId);

        if (comment.UserId != userId)
        {
            Console.WriteLine("You can only update your own comments.");
            return;
        }

        Console.WriteLine("Enter new comment body:");
        var body = Console.ReadLine();

        comment.Body = body;
        await commentRepo.UpdateCommentAsync(comment);

        Console.WriteLine("Comment updated successfully.");
    }

    private async Task DeleteCommentAsync()
    {
        Console.WriteLine("Enter comment ID:");
        var commentId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException("Invalid Comment ID"));

        await commentRepo.DeleteCommentAsync(commentId);

        Console.WriteLine("Comment deleted successfully.");
    }
}