using PlayerWallet.Model.Domain.Players;
using PlayerWallet.Model.Domain.Wallets.ApiModels;

namespace PlayerWallet.Model.Domain.Wallets.Services
{
    /// <summary>
    /// Interface for the Wallet Transaction Service.
    /// </summary>
    public interface IWalletTransactionService
    {
        /// <summary>
        /// Retrieves all transactions for a specific player.
        /// </summary>
        /// <param name="playerId">The unique identifier of the player.</param>
        /// <returns>An enumerable collection of <see cref="WalletTransactionResponse"/> objects.</returns>
        IEnumerable<WalletTransactionResponse> GetTransactions(Guid playerId);

        /// <summary>
        /// Executes a wallet transaction for a specific player.
        /// </summary>
        /// <param name="player">The player object.</param>
        /// <param name="transactionId">The unique identifier of the transaction.</param>
        /// <param name="transactionType">The type of the wallet transaction.</param>
        /// <param name="amount">The amount involved in the transaction.</param>
        /// <returns>A <see cref="WalletTransactionState"/> object containing the result of the transaction.</returns>
        WalletTransactionState ExecuteTransaction(Player player, Guid transactionId, WalletTransactionType transactionType, decimal amount);
    }
}