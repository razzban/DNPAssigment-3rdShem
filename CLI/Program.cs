using CLI.UI;
using Entities;
using InMemoryRepositories;
using RepositoryContracts;

Console.WriteLine("Welcome to the CLI!- Starting....");

// Instantiate repositories
IUserRepository userRepository = new UserInMemoryRepository();
IPostRepository postRepository = new PostInMemoryRepository();
ICommentRepository commentRepository = new CommentInMemoryRepository();

// this dummy data is created by ChatGPT: https://app.chatgpt.com/
// Add the dummy data
await SeedData(userRepository, postRepository, commentRepository);

// Initialize and start the CLI App
CliApp cLiApp = new CliApp(userRepository, postRepository, commentRepository);
await cLiApp.StartAsync();

static async Task SeedData(IUserRepository userRepo, IPostRepository postRepo, ICommentRepository commentRepo)
{
    // Add 5 dummy users (IDs will be automatically assigned)
    var users = new List<User>
    {
        new User { UserName = "Alice", Password = "password1" },
        new User { UserName = "Bob", Password = "password2" },
        new User { UserName = "Charlie", Password = "password3" },
        new User { UserName = "David", Password = "password4" },
        new User { UserName = "Eve", Password = "password5" }
    };

    foreach (var user in users)
    {
        await userRepo.AddAsync(user);
    }

    // Add 5 dummy posts
    var posts = new List<Post>
    {
        new Post { Title = "Post 1", Body = "This is post 1", UserId = 1 },
        new Post { Title = "Post 2", Body = "This is post 2", UserId = 2 },
        new Post { Title = "Post 3", Body = "This is post 3", UserId = 3 },
        new Post { Title = "Post 4", Body = "This is post 4", UserId = 4 },
        new Post { Title = "Post 5", Body = "This is post 5", UserId = 5 }
    };

    foreach (var post in posts)
    {
        await postRepo.AddAsync(post);
    }

    // Add 5 dummy comments and associate them with the correct posts
    var comments = new List<Comment>
    {
        new Comment { PostId = 1, UserId = 2, Body = "This is a comment on Post 1" },
        new Comment { PostId = 2, UserId = 3, Body = "This is a comment on Post 2" },
        new Comment { PostId = 3, UserId = 4, Body = "This is a comment on Post 3" },
        new Comment { PostId = 4, UserId = 5, Body = "This is a comment on Post 4" },
        new Comment { PostId = 5, UserId = 1, Body = "This is a comment on Post 5" }
    };

    foreach (var comment in comments)
    {
        await commentRepo.AddAsync(comment);

        // Associate the comment with the corresponding post
        var post = await postRepo.GetSingleAsync(comment.PostId);
        post.Comments.Add(comment);
        await postRepo.UpdateAsync(post);  // Ensure the post is updated with the new comment
    }

    Console.WriteLine("Dummy data seeded successfully.");
}

