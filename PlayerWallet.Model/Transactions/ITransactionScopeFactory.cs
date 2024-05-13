namespace PlayerWallet.Model.Transactions
{
    /// <summary>
    /// Interface for the Transaction Scope Factory.
    /// This interface provides methods to execute actions and functions within a unit of work.
    /// </summary>
    public interface ITransactionScopeFactory
    {
        /// <summary>
        /// Executes the provided action within a unit of work.
        /// </summary>
        /// <param name="playerId">The unique identifier of the player.</param>
        /// <param name="callback">The action to be executed.</param>
        void ExecuteAsUnitOfWork(Guid playerId, Action callback);

        /// <summary>
        /// Executes the provided function within a unit of work and returns the result.
        /// </summary>
        /// <typeparam name="T">The type of the return value.</typeparam>
        /// <param name="playerId">The unique identifier of the player.</param>
        /// <param name="callback">The function to be executed.</param>
        /// <returns>The result of the function execution.</returns>
        T ExecuteAsUnitOfWork<T>(Guid playerId, Func<T> callback);
    }
}