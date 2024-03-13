using System;
namespace Server.Models
{
	public class Post
	{
        public int Id { get; set; }

        public string Author { get; set; } = string.Empty;

        public int AuthorId { get; set; }

        public int Likes { get; set; }

        public double Popularity { get; set; }

        public int Reads { get; set; }

        public List<string>? Tags { get; set; }
    }
}

