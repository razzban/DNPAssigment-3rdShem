using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using RepositoryContracts;

namespace InMemoryRepositories
{
    public class PostInMemoryRepository : IPostRepository
    {
        public readonly List<Post> Posts = new List<Post>();  // Correct initialization

        public Task<Post> AddAsync(Post post)
        {
            if (Posts.Any(p => p.Title == post.Title))
            {
                throw new InvalidOperationException($"A post with the title '{post.Title}' already exists.");
            }

            post.Id = Posts.Count != 0 ? Posts.Max(p => p.Id) + 1 : 1;
            Posts.Add(post);
            return Task.FromResult(post);
        }

        public Task UpdateAsync(Post post)
        {
            var existingPost = Posts.SingleOrDefault(p => p.Id == post.Id);
            if (existingPost == null)
            {
                throw new InvalidOperationException($"Post with ID '{post.Id}' not found.");
            }

            // Directly update the fields of the existing post
            existingPost.Title = post.Title;
            existingPost.Body = post.Body;
            existingPost.UserId = post.UserId;

            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            var postToRemove = Posts.SingleOrDefault(p => p.Id == id);
            if (postToRemove == null)
            {
                throw new InvalidOperationException($"Post with ID '{id}' not found.");
            }

            Posts.Remove(postToRemove);
            return Task.FromResult(true);  // Return true to indicate successful deletion
        }

        public Task<Post> GetSingleAsync(int id)
        {
            var post = Posts.SingleOrDefault(p => p.Id == id);
            if (post == null)
            {
                throw new InvalidOperationException($"Post with ID '{id}' not found.");
            }

            return Task.FromResult(post);
        }

        public IQueryable<Post> GetMany()
        {
            return Posts.AsQueryable();
        }
        
       

    }
}
