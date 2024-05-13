using Microsoft.AspNetCore.OutputCaching;

namespace PlayerWallet.Model.Caching;

public class CacheService(IOutputCacheStore outputCacheStore) : ICacheService
{
    public async Task ClearOutputCache(string tag)
    {
        await outputCacheStore.EvictByTagAsync(tag, default);
    }
}