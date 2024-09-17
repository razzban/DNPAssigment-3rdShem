using CLI.UI;
using Entities;
using InMemoryRepositories;
using RepositoryContracts;

Console.WriteLine("Welcome to the CLI!- Starting....");

// Initialize the repositories
IUserRepository userRepository = new UserInMemoryRepository();
IPostRepository postRepository = new PostInMemoryRepository();
ICommentRepository commentRepository = new CommentInMemoryRepository();

// this dummy data is created by ChatGPT: https://app.chatgpt.com/
// Add the dummy data
await SeedData(userRepository, postRepository, commentRepository);

// Initialize and start the CLI App
var cLiApp = new CliApp(userRepository, postRepository, commentRepository);
await cLiApp.StartAsync();
return;

static async Task SeedData(IUserRepository userRepo, IPostRepository postRepo, ICommentRepository commentRepo)
{
    // Add 5 dummy users (IDs will be automatically assigned)
    var users = new List<User>
    {
        new User { UserId = 1, UserName = "Alice", Password = "password1" },
        new User { UserId = 2, UserName = "Bob", Password = "password2" },
        new User { UserId = 3, UserName = "Charlie", Password = "password3" },
        new User { UserId = 4, UserName = "David", Password = "password4" },
        new User { UserId = 5, UserName = "Eve", Password = "password5" }
    };

    foreach (var user in users)
    {
        await userRepo.AddUserAsync(user);
    }

    // Add 5 dummy posts
    var posts = new List<Post>
    {
        new Post { PostId = 1,Title = "Post 1", Body = "This is post 1", UserId = 1 },
        new Post { PostId = 2, Title = "Post 2", Body = "This is post 2", UserId = 2 },
        new Post { PostId = 3, Title = "Post 3", Body = "This is post 3", UserId = 3 },
        new Post { PostId = 4, Title = "Post 4", Body = "This is post 4", UserId = 4 },
        new Post { PostId = 5, Title = "Post 5", Body = "This is post 5", UserId = 5 }
    };

    foreach (var post in posts)
    {
        await postRepo.AddPostAsync(post);
    }

    // Add 5 dummy comments and associate them with the correct posts
    var comments = new List<Comment>
    {
        new Comment { CommentId = 1, PostId = 1, UserId = 2, Body = "This is a comment on Post 1" },
        new Comment { CommentId = 2, PostId = 2, UserId = 3, Body = "This is a comment on Post 2" },
        new Comment { CommentId = 3, PostId = 3, UserId = 4, Body = "This is a comment on Post 3" },
        new Comment { CommentId = 4, PostId = 4, UserId = 5, Body = "This is a comment on Post 4" },
        new Comment { CommentId = 5, PostId = 5, UserId = 1, Body = "This is a comment on Post 5" }
    };

    foreach (var comment in comments)
    {
        await commentRepo.AddCommentAsync(comment);

        // Associate the comment with the corresponding post
        var post = await postRepo.GetSinglePostAsync(comment.PostId);
        post.Comments.Add(comment);
        await postRepo.UpdatePostAsync(post);  // Ensure the post is updated with the new comment
    }

    Console.WriteLine("Dummy data seeded successfully.");
}

