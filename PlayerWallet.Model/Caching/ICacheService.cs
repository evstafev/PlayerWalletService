namespace PlayerWallet.Model.Caching
{
    /// <summary>
    /// Interface for the Cache Service.
    /// This interface provides methods to manage the output cache.
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Clears the output cache for a specific tag.
        /// </summary>
        /// <param name="tag">The tag associated with the cache entries to clear.</param>
        Task ClearOutputCache(string tag);
    }
}