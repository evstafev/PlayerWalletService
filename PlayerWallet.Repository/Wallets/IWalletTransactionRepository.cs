using PlayerWallet.Repository.Wallets.DataModels;

namespace PlayerWallet.Repository.Wallets
{
    /// <summary>
    /// Interface for the Wallet Transaction Repository.
    /// </summary>
    public interface IWalletTransactionRepository
    {
        /// <summary>
        /// Retrieves a specific transaction from the repository.
        /// </summary>
        /// <param name="transactionId">The unique identifier of the transaction.</param>
        /// <returns>A <see cref="WalletTransactionDb"/> object if found, null otherwise.</returns>
        WalletTransactionDb? GetTransaction(Guid transactionId);

        /// <summary>
        /// Retrieves all transactions for a specific player from the repository.
        /// </summary>
        /// <param name="playerId">The unique identifier of the player.</param>
        /// <returns>An enumerable collection of <see cref="WalletTransactionDb"/> objects.</returns>
        IEnumerable<WalletTransactionDb> GetTransactions(Guid playerId);

        /// <summary>
        /// Saves a transaction to the repository.
        /// </summary>
        /// <param name="transaction">The <see cref="WalletTransactionDb"/> object to save.</param>
        void SaveTransaction(WalletTransactionDb transaction);
    }
}