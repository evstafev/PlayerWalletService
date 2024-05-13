namespace PlayerWallet.Repository.Players
{
    /// <summary>
    /// Provides an interface for managing player data in the repository.
    /// </summary>
    public interface IPlayerRepository
    {
        /// <summary>
        /// Creates a new player in the repository.
        /// </summary>
        /// <param name="playerId">The unique identifier of the player.</param>
        /// <returns>True if the player was successfully created, false otherwise.</returns>
        bool CreatePlayer(Guid playerId);

        /// <summary>
        /// Retrieves the balance of a specific player from the repository.
        /// </summary>
        /// <param name="playerId">The unique identifier of the player.</param>
        /// <returns>The balance of the player as a decimal. Null if the player does not exist or has no balance.</returns>
        decimal? GetBalance(Guid playerId);

        /// <summary>
        /// Checks if a player exists in the repository.
        /// </summary>
        /// <param name="playerId">The unique identifier of the player.</param>
        /// <returns>True if the player exists, false otherwise.</returns>
        bool PlayerExists(Guid playerId);

        /// <summary>
        /// Sets the balance of a specific player in the repository.
        /// </summary>
        /// <param name="playerId">The unique identifier of the player.</param>
        /// <param name="balance">The new balance of the player.</param>
        /// <remarks>This method will throw an exception if the player does not exist.</remarks>
        void SetBalance(Guid playerId, decimal balance);
    }
}