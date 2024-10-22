namespace ApiContracts;

public class PostDto
{
    public int id { get; set; }
    public int userId { get; set; }
    public required string title { get; set; }
    public required string body { get; set; }
    
}