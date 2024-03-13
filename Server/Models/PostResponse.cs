namespace Server.Models
{
    /// <summary>
    /// response type for retrieving data from database(json format)
    /// </summary>
	public class PostResponse
	{
        public IEnumerable<Post>? Posts { get; set; }
    }
}

