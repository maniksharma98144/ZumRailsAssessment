using System.Linq.Expressions;
using Server.Models;
using Server.Repositories;

namespace Server.Services
{
    public class PostService : IPostService
    {
        public readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IEnumerable<Post>> GetPosts(string[] tags, string sortBy, string direction)
        {
            List<Post> posts = new();

            IEnumerable<string> distinctTags = tags.Distinct();

            foreach (string tag in distinctTags)
            {
                posts.AddRange(await _postRepository.GetPostsAsync(tag));
            }

            var distinctPosts = posts.Distinct();

            var sortedPosts = SortPosts(distinctPosts, sortBy, direction);

            return sortedPosts;
        }

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

