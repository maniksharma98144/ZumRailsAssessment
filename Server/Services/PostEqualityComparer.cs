using Server.Models;

namespace Server.Services
{
    /// <summary>
    /// equality comparer for removing duplicate posts
    /// </summary>
    public class PostEqualityComparer : IEqualityComparer<Post>
    {
        public bool Equals(Post x, Post y)
        {
            return x != null && y != null && x.Id == y.Id;
        }

        public int GetHashCode(Post obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}

