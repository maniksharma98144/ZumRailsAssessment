using System;
namespace Server.Dtos
{
    /// <summary>
    /// PostDto for returning the data type of posts to the client
    /// </summary>
    public class PostDto
    {
        public int Id { get; set; }

        public string Author { get; set; } = String.Empty;

        public int AuthorId { get; set; }

        public int Likes { get; set; }

        public double Popularity { get; set; }

        public int Reads { get; set; }

        public List<string>? Tags { get; set; }
    }
}

