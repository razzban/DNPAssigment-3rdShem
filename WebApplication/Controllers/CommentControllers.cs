namespace WebApplication.Controllers;

using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

[ApiController]
[Route("comments")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;

    public CommentsController(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    [HttpPost]
    public async Task<IResult> CreateAsync(CreateCommentDto commentDto)
    {
        Comment comment = await _commentRepository.AddCommentAsync(new Comment
        {
            Body = commentDto.Body, UserId = commentDto.UserId, PostId = commentDto.PostId
        });
        return Results.Created($"comments/{comment.CommentId}", comment);
    }

    [HttpGet("{id:int}")]
    public async Task<IResult> GetSingleAsync([FromRoute] int id)
    {
        Comment comment = await _commentRepository.GetSingleCommentAsync(id);
        return Results.Ok(new CommentDto { Id = comment.CommentId, Body = comment.Body });
    }

    [HttpGet]
    public async Task<IResult> GetManyAsync([FromQuery] int? userId, int? postId)
    {
        IQueryable<Comment> comments = _commentRepository.GetCommentMany();
        if (userId is not null)
        {
            comments = comments.Where(c => c.UserId.Equals(userId));
        }

        if (postId is not null)
        {
            comments = comments.Where(c => c.PostId.Equals(postId));
        }

        IQueryable<CommentDto> commentDtos = comments.Select(c => new CommentDto
        {
            Id = c.CommentId, Body = c.Body, UserId = c.UserId, PostId = c.PostId
        });
        return Results.Ok(commentDtos);
    }

    [HttpPut("{id:int}")]
    public async Task<IResult> UpdateAsync(int id, CreateCommentDto commentDto)
    {
        await _commentRepository.UpdateCommentAsync(new Comment
        {
            CommentId = id, Body = commentDto.Body, UserId = commentDto.UserId, PostId = commentDto.PostId
        });
        return Results.Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IResult> DeleteAsync(int id)
    {
        await _commentRepository.DeleteCommentAsync(id);
        return Results.Ok();
    }
}