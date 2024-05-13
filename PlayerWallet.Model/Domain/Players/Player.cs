namespace PlayerWallet.Model.Domain.Players
{
    /// <summary>
    /// Represents a player in the system.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the player.</param>
        /// <param name="balance">The balance of the player's wallet. Null if the player does not have a wallet.</param>
        public Player(Guid id, decimal? balance)
        {
            Id = id;
            Balance = balance;
        }

        /// <summary>
        /// Gets the unique identifier of the player.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets the balance of the player's wallet. Null if the player does not have a wallet.
        /// </summary>
        public decimal? Balance { get; }

        /// <summary>
        /// Gets a value indicating whether the player has a wallet.
        /// </summary>
        public bool HasWallet => Balance.HasValue;
    }
}