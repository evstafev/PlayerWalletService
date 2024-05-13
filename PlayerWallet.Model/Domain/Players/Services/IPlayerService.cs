using PlayerWallet.Model.Domain.Players.ApiModels;
using PlayerWallet.Model.Domain.Wallets;

namespace PlayerWallet.Model.Domain.Players.Services
{
    /// <summary>
    /// Interface for the Player Service.
    /// </summary>
    public interface IPlayerService
    {
        /// <summary>
        /// Adds a new player to the system.
        /// </summary>
        /// <returns>A new <see cref="PlayerResponse"/> object.</returns>
        PlayerResponse CreatePlayer();

        /// <summary>
        /// Retrieves the balance of a specific player.
        /// </summary>
        /// <param name="playerId">The unique identifier of the player.</param>
        /// <returns>A <see cref="PlayerResponse"/> object containing the player's balance.</returns>
        PlayerResponse GetBalance(Guid playerId);

        /// <summary>
        /// Registers a wallet for a specific player.
        /// </summary>
        /// <param name="playerId">The unique identifier of the player.</param>
        void RegisterWallet(Guid playerId);

        /// <summary>
        /// Executes a wallet operation for a specific player.
        /// </summary>
        /// <param name="playerId">The unique identifier of the player.</param>
        /// <param name="transactionId">The unique identifier of the transaction.</param>
        /// <param name="transactionType">The type of the wallet transaction.</param>
        /// <param name="amount">The amount involved in the transaction.</param>
        /// <returns>A <see cref="TransactionResponse"/> object containing the result of the transaction.</returns>
        TransactionResponse ExecuteWalletOperation(Guid playerId, Guid transactionId, WalletTransactionType transactionType, decimal amount);
    }
}