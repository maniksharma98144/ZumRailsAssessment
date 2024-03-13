using Microsoft.Extensions.Caching.Memory;
using System.Threading;
using Newtonsoft.Json;
using Server.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace Server.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        private readonly IMemoryCache _cache;
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public PostRepository(HttpClient httpClient, string apiBaseUrl, IMemoryCache memoryCache)
        {
            if (string.IsNullOrEmpty(apiBaseUrl))
            {
                throw new ArgumentException($"'{nameof(apiBaseUrl)}' cannot be null or empty.", nameof(apiBaseUrl));
            }

            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiBaseUrl = apiBaseUrl;
            _cache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        private string CreateTaggedUrl(string tag) => $"{_apiBaseUrl}?tag={Uri.EscapeDataString(tag)}";

        public async Task<IEnumerable<Post>> GetPostsAsync(string tag)
        {
            try
            {
                await semaphore.WaitAsync();

                if (_cache.TryGetValue(tag, out PostResponse? posts))
                {
                    Console.WriteLine("in memory");
                    return posts.Posts;
                }
                Console.WriteLine("out memory");

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal)
                        .SetSize(1024);

                string Url = CreateTaggedUrl(tag);
                var response = await _httpClient.GetAsync(Url);
                var content = await response.Content.ReadAsStringAsync();
                PostResponse? data = JsonConvert.DeserializeObject<PostResponse>(content);
                if (data == null)
                {
                    return new List<Post>();
                }
                _cache.Set(tag, data, cacheOptions);
                return data.Posts;
            }

            finally
            {
                semaphore.Release();
            }
        }
    }
}

