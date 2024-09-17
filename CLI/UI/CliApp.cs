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
            Console.WriteLine("3. Manage Comments");
            Console.WriteLine("4. Filter Search");
            Console.WriteLine("5. Exit");

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
                    await ManageCommentAsync();
                    break;
                case "4":
                    await FilterSearchAsync();
                    break;
                case "5":
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
        //enter user details
        Console.WriteLine("Enter user ID:");
        var id = int.Parse(Console.ReadLine() ?? string.Empty);
        Console.WriteLine("Enter username:");
        var username = Console.ReadLine();
        Console.WriteLine("Enter password:");
        var password = Console.ReadLine();
        
        //create new user
        var newUser = new User { UserId= id ,UserName = username, Password = password };
        //add user to the database
        try
        {
            //add user to the database
            await userRepo.AddUserAsync(newUser);  
            Console.WriteLine("User created successfully.");
        }
        //catch exception if user already exists
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");  
        }

        Console.WriteLine("User created successfully.");
    }
    
    private void ViewUsers()
    {
        //get all users
        var users = userRepo.GetUserMany();
        //display all users
        foreach (var user in users)
        {
            Console.WriteLine($"ID: {user.UserId}, Name: {user.UserName}");
        }
    }

    private async Task UpdateUserAsync()
    {
        //enter User id to update
        Console.WriteLine("Enter user ID to Update:");
        var userId = int.Parse(Console.ReadLine() ?? string.Empty);
        
        //get the user by id
        var user = await userRepo.GetSingleUserAsync(userId);
        
        //new users and password:
        Console.WriteLine("Enter new username:");
        var username = Console.ReadLine();
        Console.WriteLine("Enter new password:");
        var password = Console.ReadLine();
        
        //update the user
        user.UserName = username;
        user.Password = password;
        //update the user in the database
        await userRepo.UpdateUserAsync(user);

        Console.WriteLine("User updated successfully.");
    }
    
    private async Task DeleteUserAsync()
    {
        //enter User id to delete
        Console.WriteLine("Enter user ID to Delete:");
        var userId = int.Parse(Console.ReadLine() ?? string.Empty);
        
        //delete the user
        await userRepo.DeleteUserAsync(userId);

        Console.WriteLine("User deleted successfully.");
    }
    
    
    
    
    private async Task ManagePostsAsync()
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
        //enter post details
        Console.WriteLine("Enter post title:");
        var title = Console.ReadLine();
        Console.WriteLine("Enter post body:");
        var body = Console.ReadLine();
        Console.WriteLine("Enter user ID:");
        var userId = int.Parse(Console.ReadLine() ?? string.Empty);

        //create new post
        var newPost = new Post { Title = title, Body = body, UserId = userId };
        //add post to the database
        await postRepo.AddPostAsync(newPost);

        Console.WriteLine("Post created successfully.");
    }

    private void ViewPosts()
    {
        //get all posts
        var posts = postRepo.GetPostMany();

        //display all posts
        foreach (var post in posts)
        {
            Console.WriteLine($"Post ID: {post.PostId}, Title: {post.Title}, Body: {post.Body}");

            // Display comments for the post if there is any
            if (post.Comments.Count != 0)
            {
                // Display comments
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
        //enter post id to update
        Console.WriteLine("Enter post ID to Update:");
        var postId = int.Parse(Console.ReadLine() ?? string.Empty);
        
        //get the post by id
        var post = await postRepo.GetSinglePostAsync(postId);
        
        //new post title and body:
        Console.WriteLine("Enter new post title:");
        var title = Console.ReadLine();
        Console.WriteLine("Enter new post body:");
        var body = Console.ReadLine();
        
        //update the post
        post.Title = title;
        post.Body = body;
        //update the post in the database
        await postRepo.UpdatePostAsync(post);

        Console.WriteLine("Post updated successfully.");
    }
    
    private async Task DeletePostAsync()
    {
        //enter post id to delete
        Console.WriteLine("Enter post ID to Delete:");
        var postId = int.Parse(Console.ReadLine() ?? string.Empty);
        
        //delete  post
        await postRepo.DeletePostAsync(postId);

        Console.WriteLine("Post deleted successfully.");
    }
    
    
    
    
    private  async Task ManageCommentAsync ()
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
        // Get post ID from the user
        Console.WriteLine("Enter post ID:");
        var postId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException("Invalid Post ID"));
        var post = await postRepo.GetSinglePostAsync(postId);
        // Get the user ID from the user
        Console.WriteLine("Enter user ID:");
        var userId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException("Invalid User ID"));
        // Get the comment body from the user
        Console.WriteLine("Enter comment body:");
        var body = Console.ReadLine();

        // Create a new comment
        var newComment = new Comment { PostId = postId, UserId = userId, Body = body };
        // Add the comment to the database
        await commentRepo.AddCommentAsync(newComment);
        // Add the comment to the post
        post.Comments.Add(newComment);
        // Update the post in the database
        await postRepo.UpdatePostAsync(post);
        Console.WriteLine("Comment added successfully.");
    }
    
    private async Task UpdateCommentAsync()
    {
        // Get comment ID from the user
        Console.WriteLine("Enter comment ID:");
        var commentId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException("Invalid Comment ID"));

        // Get the user ID from the user
        Console.WriteLine("Enter your user ID:");
        var userId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException("Invalid User ID"));

        // Retrieve the comment from the repository
        var comment = await commentRepo.GetSingleCommentAsync(commentId);
    
        // Check if the comment exists
        if (comment == null)
        {
            Console.WriteLine($"Comment with ID {commentId} not found.");
            return;
        }

        // Check if the current user is the owner of the comment
        if (comment.UserId != userId)
        {
            Console.WriteLine("You can only update your own comments.");
            return;
        }

        // Get the new comment body from the user
        Console.WriteLine("Enter new comment body:");
        var body = Console.ReadLine();

        // Update the comment body
        comment.Body = body;

        // Update the comment in the repository
        await commentRepo.UpdateCommentAsync(comment);
    
        Console.WriteLine("Comment updated successfully.");
    }

    
    private async Task DeleteCommentAsync()
    {
        // Get comment ID from the user
        Console.WriteLine("Enter comment ID:");
        var commentId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException("Invalid Comment ID"));
        // Get the comment from the database
        var comment = await commentRepo.GetSingleCommentAsync(commentId);
        // Delete the comment from the database
        await commentRepo.DeleteCommentAsync(commentId);
        Console.WriteLine("Comment deleted successfully.");
    }
    
    private async Task FilterSearchAsync()
    {
        Console.WriteLine("1. Search all posts by User ID");
        Console.WriteLine("2. Search all comments by User ID");
        Console.WriteLine("3. all users with specific name in username");
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
        // Get user ID from the user
        Console.WriteLine("Enter user ID:");
        var userId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException("Invalid User ID"));
        // Get all posts by the user
        var posts = postRepo.GetPostMany().Where(p => p.UserId == userId);
        // Display the posts
        foreach (var post in posts)
        {
            Console.WriteLine($"Post ID: {post.PostId}, Title: {post.Title}, Body: {post.Body}");
        }
    }
    
    private async Task SearchCommentsByUserIdAsync()
    {
        // Get user ID from the user
        Console.WriteLine("Enter user ID:");
        var userId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException("Invalid User ID"));
        // Get all comments by the user
        var comments = commentRepo.GetCommentMany().Where(c => c.UserId == userId);
        // Display the comments
        foreach (var comment in comments)
        {
            Console.WriteLine($"Comment ID: {comment.CommentId}, Post ID: {comment.PostId}, Body: {comment.Body}");
        }
    }
    
    private async Task SearchUsersByNameAsync()
    {
        // Get Keyword from the user
        Console.WriteLine("Enter Keyword:");
        var keyword = Console.ReadLine();
        // Get all users with the keyword in the username
        var users = userRepo.GetUserMany().Where(u => u.UserName.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
        // Display the users
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
    }

}

