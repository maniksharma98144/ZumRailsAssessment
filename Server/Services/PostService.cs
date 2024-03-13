using System.Linq.Expressions;
using Server.Models;
using Server.Repositories;

namespace Server.Services
{
    public class PostService : IPostService
    {
        public readonly IPostRepository _postRepository;
        private readonly IEqualityComparer<Post> _postEqualityComparer;

        public PostService(IPostRepository postRepository, IEqualityComparer<Post> postEqualityComparer)
        {
            _postRepository = postRepository;
            _postEqualityComparer = postEqualityComparer;
        }

        /// <summary>
        /// helper method to get posts
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="sortBy"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Post>> GetPosts(string[] tags, string sortBy, string direction)
        {
            List<Post> posts = new();

            IEnumerable<string> uniqueTags = tags.Distinct();

            foreach (string tag in uniqueTags)
            {
                posts.AddRange(await _postRepository.GetPostsAsync(tag));
            }

            var uniquePosts = posts.Distinct(_postEqualityComparer);

            var sortedPosts = SortPosts(uniquePosts, sortBy, direction);

            return sortedPosts;
        }

        /// <summary>
        /// helper method to sort posts based on sortby and direction
        /// </summary>
        /// <param name="posts"></param>
        /// <param name="sortBy"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static List<Post> SortPosts(IEnumerable<Post> posts, string sortBy, string direction)
        {
            if (posts == null || !posts.Any())
                return new List<Post>();

            var sortExpression = GetExpression(sortBy);

            return direction?.ToLower() switch
            {
                "desc" => posts.OrderByDescending(sortExpression.Compile()).ToList(),
                "asc" => posts.OrderBy(sortExpression.Compile()).ToList(),
                _ => throw new ArgumentException("Direction parameter is invalid"),
            };
        }

        /// <summary>
        /// helper methdo to generate expressions for sorting
        /// </summary>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static Expression<Func<Post, object>> GetExpression(string? sortBy)
        {
            Expression<Func<Post, object>> expression = sortBy?.ToLower() switch
            {
                "id" => post => post.Id,
                "reads" => post => post.Reads,
                "likes" => post => post.Likes,
                "popularity" => post => post.Popularity,
                _ => throw new ArgumentException("sortBy parameter is invalid"),
            };
            return expression;
        }
    }
}

