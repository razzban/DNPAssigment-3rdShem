using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{
    private const string FilePath = "comments.json";

    // Path to the file where comments are stored
    public CommentFileRepository()
    {
        if (!File.Exists(FilePath))
        {
            File.WriteAllText(FilePath, "[]");  // Initialize with an empty JSON array
        }
    }

    public async Task<Comment> AddCommentAsync(Comment comment)
    {
        // Read the current list of comments from the file
        var commentsAsJson = await File.ReadAllTextAsync(FilePath);
        var comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;

        // Set the new CommentId
        comment.CommentId = comments.Count > 0 ? comments.Max(c => c.CommentId) + 1 : 1;
        comments.Add(comment);

        // Serialize the updated list back to the file
        commentsAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(FilePath, commentsAsJson);

        return comment;
    }

    public async Task UpdateCommentAsync(Comment comment)
    {
        // Read the current list of comments from the file
        var commentsAsJson = await File.ReadAllTextAsync(FilePath);
        var comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;

        // Find the comment by CommentId
        var existingComment = comments.SingleOrDefault(c => c.CommentId == comment.CommentId);
        if (existingComment == null)
        {
            throw new InvalidOperationException($"Comment with ID '{comment.CommentId}' not found");
        }

        // Update the comment's fields
        existingComment.Body = comment.Body;
        existingComment.UserId = comment.UserId;

        // Serialize the updated list back to the file
        commentsAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(FilePath, commentsAsJson);
    }

    public async Task DeleteCommentAsync(int id)
    {
        // Read the current list of comments from the file
        var commentsAsJson = await File.ReadAllTextAsync(FilePath);
        var comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;

        // Find the comment to remove by ID
        var commentToRemove = comments.SingleOrDefault(c => c.CommentId == id);
        if (commentToRemove == null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found");
        }

        // Remove the comment from the list
        comments.Remove(commentToRemove);

        // Serialize the updated list back to the file
        commentsAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(FilePath, commentsAsJson);
    }

    public async Task<Comment> GetSingleCommentAsync(int id)
    {
        // Read the current list of comments from the file
        var commentsAsJson = await File.ReadAllTextAsync(FilePath);
        var comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;

        // Find the comment by ID
        var comment = comments.SingleOrDefault(c => c.CommentId == id);
        if (comment == null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found");
        }

        return comment;
    }

    public IQueryable<Comment> GetCommentMany()
    {
        // Read the current list of comments from the file
        var commentsAsJson = File.ReadAllTextAsync(FilePath).Result;
        var comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;

        // Return the list of comments as a queryable collection
        return comments.AsQueryable();
    }
}
