using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApplication.Controllers;

[ApiController]
[Route("posts")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository _postRepository;

    public PostsController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    // Create a new post
    [HttpPost]
    public async Task<IResult> CreateAsync(CreatePostDto postDto)
    {
        Post createdPost = await _postRepository.AddPostAsync(new Post
        {
            UserId = postDto.UserId,
            Title = postDto.Title,
            Body = postDto.Body
        });
        return Results.Created($"posts/{createdPost.PostId}", createdPost);
    }

    // Get a single post by ID
    [HttpGet("{id:int}")]
    public async Task<IResult> GetSingleAsync(int id)
    {
        Post post = await _postRepository.GetSinglePostAsync(id);
        var postDto = new PostDto
        {
            PostId = post.PostId,
            UserId = post.UserId,
            Title = post.Title,
            Body = post.Body
        };
        return Results.Ok(postDto);
    }

    // Get multiple posts, optionally filtering by userId
    [HttpGet]
    public async Task<IResult> GetManyAsync([FromQuery] int? userId)
    {
        IQueryable<Post> posts = _postRepository.GetPostMany();

        if (userId is not null)
        {
            posts = posts.Where(p => p.UserId == userId);
        }

        IQueryable<PostDto> postDtos = posts.Select(p => new PostDto
        {
            PostId = p.PostId,
            UserId = p.UserId,
            Title = p.Title,
            Body = p.Body
        });

        return Results.Ok(postDtos);
    }

    // Update an existing post
    [HttpPut("{id:int}")]
    public async Task<IResult> UpdateAsync(int id, CreatePostDto postDto)
    {
        await _postRepository.UpdatePostAsync(new Post
        {
            PostId = id,
            UserId = postDto.UserId,
            Title = postDto.Title,
            Body = postDto.Body
        });
        return Results.Ok();
    }

    // Delete a post by ID
    [HttpDelete("{id:int}")]
    public async Task<IResult> DeleteAsync(int id)
    {
        await _postRepository.DeletePostAsync(id);
        return Results.Ok();
    }
}
