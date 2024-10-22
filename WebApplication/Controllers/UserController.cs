namespace WebApplication.Controllers;


using ApiContracts;

using Entities;

using Microsoft.AspNetCore.Mvc;

using RepositoryContracts;


[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost]
    public async Task<IResult> CreateAsync(CreateUserDto userDto)
    {
        var createdUser = await _userRepository.AddUserAsync(new User
        {
            UserName = userDto.Name, Password = userDto.Password
        });
        return Results.Created($"users/{createdUser.UserId}", createdUser);
    }

    [HttpGet("{id:int}")]
    public async Task<IResult> GetSingleAsync(int id)
    {
        var user = await _userRepository.GetSingleUserAsync(id);
        return Results.Ok(new UserDto { Id = user.UserId, Name = user.UserName });
    }

    [HttpGet]
    public async Task<IResult> GetManyAsync([FromQuery] string? name)
    {
        var users = _userRepository.GetUserMany();

        // Filter users by the "name" query parameter
        if (!string.IsNullOrEmpty(name))
        {
            users = users.Where(u => u.UserName != null && u.UserName.ToLower().Contains(name.ToLower()));
        }

        var userDtos = users.Select(u => new UserDto { Id = u.UserId, Name = u.UserName});

        return Results.Ok(userDtos);
    }

    [HttpPut("{id:int}")]
    public async Task<IResult> UpdateAsync(int id, CreateUserDto userDto)
    {
        await _userRepository.UpdateUserAsync(new User { UserId = id, UserName = userDto.Name, Password = userDto.Password });
        return Results.Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IResult> DeleteAsync(int id)
    {
        await _userRepository.DeleteUserAsync(id);
        return Results.Ok();
    }
}