using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OutputCaching;

namespace PlayerWallet.Model.Caching;

/// <summary>
/// Defines the output cache policy for player's balance related requests.
/// </summary>
public class PlayerOutputCachePolicy : IOutputCachePolicy
{
    public PlayerOutputCachePolicy()
    {
    }

    /// <summary>
    /// Configures the cache policy for the current request.
    /// </summary>
    public ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellation)
    {
        var attemptOutputCaching = AttemptOutputCaching(context);
        context.EnableOutputCaching = true;
        context.AllowCacheLookup = attemptOutputCaching;
        context.AllowCacheStorage = attemptOutputCaching;
        context.AllowLocking = true;
        context.CacheVaryByRules.QueryKeys = "*";

        context.ResponseExpirationTimeSpan = TimeSpan.FromDays(1);

        // Mark the cache for each player with the playerId.
        var playerId = context.HttpContext.Request.RouteValues.GetValueOrDefault("playerId") as string;

        if (!string.IsNullOrWhiteSpace(playerId))
        {
            context.Tags.Add(playerId);
        }

        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Handles serving the response from cache.
    /// </summary>
    public ValueTask ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellation)
    {
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Handles serving the response and determines if the response should be stored in cache.
    /// </summary>
    public ValueTask ServeResponseAsync(OutputCacheContext context, CancellationToken cancellation)
    {
        var response = context.HttpContext.Response;

        // Check response code
        if (response.StatusCode == StatusCodes.Status200OK) return ValueTask.CompletedTask;

        context.AllowCacheStorage = false;
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Determines if the current request should be cached.
    /// </summary>
    private static bool AttemptOutputCaching(OutputCacheContext context)
    {
        // Check if the current request fulfills the requirements
        // to be cached
        var request = context.HttpContext.Request;

        // Verify the method
        return HttpMethods.IsGet(request.Method);
    }
}