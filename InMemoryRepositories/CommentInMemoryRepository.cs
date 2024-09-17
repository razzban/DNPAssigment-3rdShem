
using Entities;
using RepositoryContracts;

namespace InMemoryRepositories
{
    // Implement the ICommentRepository interface
    public class CommentInMemoryRepository : ICommentRepository
    {
        //list of comments
        private readonly List<Comment> _comments = [];
        
        public Task<Comment> AddCommentAsync(Comment comment)
        {
            // Set the comment ID to the next available ID
            comment.CommentId = _comments.Count != 0 ? _comments.Max(c => c.CommentId) + 1 : 1;
            _comments.Add(comment);
            return Task.FromResult(comment);
        }

        public Task UpdateCommentAsync(Comment comment)
        {
            // Find the existing comment by ID
            var existingComment = _comments.SingleOrDefault(c => c.CommentId == comment.CommentId);

            // If the comment does not exist, throw an exception
            if (existingComment == null)
            {
                throw new InvalidOperationException($"Comment with ID '{comment.CommentId}' not found");
            }

            // Update the existing comment's fields 
            existingComment.Body = comment.Body;
            existingComment.UserId = comment.UserId;  // Update if userId changes (if needed)

            return Task.CompletedTask;
        }


        public Task DeleteCommentAsync(int id)
        {
            // Find the comment to remove by ID
            var commentToRemove = _comments.SingleOrDefault(c => c.CommentId == id);
            // If the comment does not exist, throw an exception
            if (commentToRemove == null)
            {
                throw new InvalidOperationException($"Comment with ID '{id}' not found");
            }
            // Remove the comment from the list
            _comments.Remove(commentToRemove);
            return Task.CompletedTask;
        }

        public Task<Comment> GetSingleCommentAsync(int id)
        {
            // Find the comment by ID and return it
            var comment = _comments.SingleOrDefault(c => c.CommentId == id);
            // If the comment does not exist, throw an exception
            if (comment == null)
            {
                throw new InvalidOperationException($"Comment with ID '{id}' not found");
            }
            return Task.FromResult(comment);
        }

        public IQueryable<Comment> GetCommentMany()
        {
            // Return the list of comments as a queryable collection
            return _comments.AsQueryable();
        }
    }
}