namespace Entities;

public class Post
{
  
    public int PostId { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
    public int UserId { get; set; } 
    //list of comments
    public List<Comment> Comments { get; set; } = [];
    
    
}