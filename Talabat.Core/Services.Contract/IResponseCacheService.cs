namespace Talabat.Core.Reposistories.Contract;

public interface IResponseCacheService
{
    Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);
    Task<string?> GetCachedResponseAsync(string cacheKey);
    // Task RemoveCacheByPattern(string pattern);
}