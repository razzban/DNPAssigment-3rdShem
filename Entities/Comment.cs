namespace Entities;

public class Comment
{
    
    public int CommentId { get; set; }
    public string? Body { get; set; } 
    public int PostId { get; init; } 
    public int UserId { get; set; } 
   
}