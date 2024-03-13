using Server.Models;

namespace Server.Services
{
    public interface IPostService
	{
        public Task<IEnumerable<Post>> GetPosts(string[] tags, string sortBy, string direction);
    }
}

