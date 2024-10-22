using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("posts")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository _postRepository;

    public PostsController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    [HttpPost]
    public async Task<IResult> CreateAsync(CreatePostDto postDto)
    {
        Post createdPost = await _postRepository.AddPostAsync(new Post
        {
            UserId = postDto.UserId, Title = postDto.Title, Body = postDto.Body
        });
        return Results.Created($"posts/{createdPost.PostId}", createdPost);
    }

    [HttpGet("{id:int}")]
    public async Task<IResult> GetSingleAsync(int id)
    {
        Post post = await _postRepository.GetSinglePostAsync(id);
        return Results.Ok(new PostDto {  userId = post.PostId, title= post.Title, body = post.Body });
    }

    [HttpGet]
    public async Task<IResult> GetManyAsync([FromQuery] int? userId)
    {
        IQueryable<Post> posts = _postRepository.GetPostMany();

        if (userId is not null)
        {
            posts = posts.Where(p => p.UserId.Equals(userId));
        }

        IQueryable<PostDto> postDtos =
            posts.Select(p => new PostDto {  = p.PostId, UserId = p.UserId, Title = p.Title, Body = p.Body });

        return Results.Ok(postDtos);
    }

    [HttpPut("{id:int}")]
    public async Task<IResult> UpdateAsync(int id, CreatePostDto postDto)
    {
        await _postRepository.UpdatePostAsync(new Post
        {
            PostId = id, UserId = postDto.UserId, Title = postDto.Title, Body = postDto.Body
        });
        return Results.Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IResult> DeleteAsync(int id)
    {
        await _postRepository.DeletePostAsync(id);
        return Results.Ok();
    }
}