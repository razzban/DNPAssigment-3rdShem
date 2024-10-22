namespace ApiContracts
{
    public class CreatePostDto
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}