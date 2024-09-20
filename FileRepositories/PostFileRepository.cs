using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private const string FilePath = "posts.json";

    // Path to the file where posts are stored
    public PostFileRepository()
    {
        if (!File.Exists(FilePath))
        {
            File.WriteAllText(FilePath, "[]");  // Initialize with an empty JSON array
        }
    }

    // Add a new post to the repository
    public async Task<Post> AddPostAsync(Post post)
    {
        // Read the current list of posts from the file
        var postsAsJson = await File.ReadAllTextAsync(FilePath);
        var posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;

        // Check if a post with the same title already exists
        if (posts.Any(p => p.Title == post.Title))
        {
            throw new InvalidOperationException($"A post with the title '{post.Title}' already exists.");
        }

        // Set the post ID to the next available ID
        post.PostId = posts.Count > 0 ? posts.Max(p => p.PostId) + 1 : 1;
        posts.Add(post);

        // Serialize the updated list back to the file
        postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(FilePath, postsAsJson);

        return post;
    }

    // Update an existing post
    public async Task UpdatePostAsync(Post post)
    {
        // Read the current list of posts from the file
        var postsAsJson = await File.ReadAllTextAsync(FilePath);
        var posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;

        // Find the existing post by ID
        var existingPost = posts.SingleOrDefault(p => p.PostId == post.PostId);
        if (existingPost == null)
        {
            throw new InvalidOperationException($"Post with ID '{post.PostId}' not found.");
        }

        // Update the post properties
        existingPost.Title = post.Title;
        existingPost.Body = post.Body;
        existingPost.UserId = post.UserId;

        // Serialize the updated list back to the file
        postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(FilePath, postsAsJson);
    }

    // Delete a post by ID
    public async Task DeletePostAsync(int id)
    {
        // Read the current list of posts from the file
        var postsAsJson = await File.ReadAllTextAsync(FilePath);
        var posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;

        // Find the post to remove by ID
        var postToRemove = posts.SingleOrDefault(p => p.PostId == id);
        if (postToRemove == null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found.");
        }

        // Remove the post from the list
        posts.Remove(postToRemove);

        // Serialize the updated list back to the file
        postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(FilePath, postsAsJson);
    }

    // Get a single post by ID
    public async Task<Post> GetSinglePostAsync(int id)
    {
        // Read the current list of posts from the file
        var postsAsJson = await File.ReadAllTextAsync(FilePath);
        var posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;

        // Find the post by ID
        var post = posts.SingleOrDefault(p => p.PostId == id);
        if (post == null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found.");
        }

        return post;
    }

    // Get many posts (as IQueryable)
    public IQueryable<Post> GetPostMany()
    {
        // Read the current list of posts from the file
        var postsAsJson = File.ReadAllTextAsync(FilePath).Result;
        var posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;

        // Return the list of posts as a queryable collection
        return posts.AsQueryable();
    }
}
