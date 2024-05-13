using System.Collections.Concurrent;
using PlayerWallet.Repository.Wallets.DataModels;

namespace PlayerWallet.Repository.Wallets;

public class WalletTransactionRepository : IWalletTransactionRepository
{
    private static readonly ConcurrentBag<WalletTransactionDb> Transactions = [];

    public WalletTransactionDb? GetTransaction(Guid transactionId)
    {
        return Transactions.FirstOrDefault(t => t.TransactionId == transactionId);
    }
    
    public IEnumerable<WalletTransactionDb> GetTransactions(Guid playerId)
    {
        return Transactions.Where(t => t.PlayerId == playerId);
    }
    
    public void SaveTransaction(WalletTransactionDb transaction)
    {
        Transactions.Add(transaction);
    }
}