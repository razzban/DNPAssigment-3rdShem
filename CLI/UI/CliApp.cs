using CLI.UI.manageUsers;
using CLI.UI.SpecificSearches;


namespace CLI.UI;
using System;
using System.Threading.Tasks;
using RepositoryContracts;


public class CliApp(IUserRepository userRepo, IPostRepository postRepo, ICommentRepository commentRepo)
{
   
    private readonly ManageUsers _manageUsers = new(userRepo);
    private readonly ManagePosts.ManagePosts _managePosts = new( postRepo);
    private readonly ManageComments.ManageComments _manageComments = new( postRepo, commentRepo);
    private readonly SpecificSearch _specificSearch = new(userRepo, commentRepo, postRepo);

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
                    await _manageUsers.ManageUsersAsync();
                    break;
                case "2":
                    await _managePosts.ManagePostsAsync();
                    break;
                case "3":
                    await _manageComments.ManageCommentAsync();
                    break;
                case "4":
                    await _specificSearch.FilterSearchAsync();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid choice, please try again.");
                    break;
            }
        }
    }
}