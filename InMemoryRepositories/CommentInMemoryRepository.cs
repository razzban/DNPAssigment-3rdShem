﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using RepositoryContracts;

namespace InMemoryRepositories
{
    public class CommentInMemoryRepository : ICommentRepository
    {
        private  List<Comment> comments = new();

        public Task<Comment> AddAsync(Comment comment)
        {
            comment.Id = comments.Any() ? comments.Max(c => c.Id) + 1 : 1;
            comments.Add(comment);
            return Task.FromResult(comment);
        }

        public Task UpdateAsync(Comment comment)
        {
            Comment? existingComment = comments.SingleOrDefault(c => c.Id == comment.Id);
            if (existingComment == null)
            {
                throw new InvalidOperationException($"Comment with ID '{comment.Id}' not found");
            }

            comments.Remove(existingComment);
            comments.Add(comment);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            Comment? commentToRemove = comments.SingleOrDefault(c => c.Id == id);
            if (commentToRemove == null)
            {
                throw new InvalidOperationException($"Comment with ID '{id}' not found");
            }

            comments.Remove(commentToRemove);
            return Task.CompletedTask;
        }

        public Task<Comment> GetSingleAsync(int id)
        {
            Comment? comment = comments.SingleOrDefault(c => c.Id == id);
            if (comment == null)
            {
                throw new InvalidOperationException($"Comment with ID '{id}' not found");
            }

            return Task.FromResult(comment);
        }

        public IQueryable<Comment> GetMany()
        {
            return comments.AsQueryable();
        }
    }
}