
using Entities;
using RepositoryContracts;

namespace InMemoryRepositories
{
    public class PostInMemoryRepository : IPostRepository
    {
        // List of posts
        private readonly List<Post> _posts = [];  
        
        public Task<Post> AddPostAsync(Post post)
        {
            // Check if a post with the same title already exists
            if (_posts.Any(p => p.Title == post.Title))
            {
                throw new InvalidOperationException($"A post with the title '{post.Title}' already exists.");
            }
            // Set the post ID to the next available ID
            post.PostId = _posts.Count != 0 ? _posts.Max(p => p.PostId) + 1 : 1;
            _posts.Add(post);
            return Task.FromResult(post);
        }

        public Task UpdatePostAsync(Post post)
        {
            // Find the existing post by ID
            var existingPost = _posts.SingleOrDefault(p => p.PostId == post.PostId);
            // If the post does not exist, throw an exception
            if (existingPost == null)
            {
                throw new InvalidOperationException($"Post with ID '{post.PostId}' not found.");
            }

            // Update the post properties
            existingPost.Title = post.Title;
            existingPost.Body = post.Body;
            existingPost.UserId = post.UserId;

            return Task.CompletedTask;
        }

        public Task DeletePostAsync(int id)
        {
            // Find the post to remove by ID
            var postToRemove = _posts.SingleOrDefault(p => p.PostId == id);
            // If the post does not exist, throw an exception
            if (postToRemove == null)
            {
                throw new InvalidOperationException($"Post with ID '{id}' not found.");
            }

            // Remove the post from the list
            _posts.Remove(postToRemove);
            return Task.FromResult(true);  
        }

        public Task<Post> GetSinglePostAsync(int id)
        {
            // Find the post by ID and return it
            var post = _posts.SingleOrDefault(p => p.PostId== id);
            // If the post does not exist, throw an exception
            if (post == null)
            {
                throw new InvalidOperationException($"Post with ID '{id}' not found.");
            }

            return Task.FromResult(post);
        }

        public IQueryable<Post> GetPostMany()
        {
            // Return the list of posts as a queryable collection
            return _posts.AsQueryable();
        }
        
       

    }
}
