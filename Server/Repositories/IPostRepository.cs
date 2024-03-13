using Server.Models;

namespace Server.Repositories
{
    public interface IPostRepository
    {
		public Task<IEnumerable<Post>> GetPostsAsync(string tag);
    }
}

