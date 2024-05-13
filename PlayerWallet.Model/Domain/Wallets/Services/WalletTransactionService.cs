using PlayerWallet.Model.Domain.Players;
using PlayerWallet.Model.Domain.Wallets.ApiModels;
using PlayerWallet.Repository.Players;
using PlayerWallet.Repository.Wallets;
using PlayerWallet.Repository.Wallets.DataModels;

namespace PlayerWallet.Model.Domain.Wallets.Services;

public class WalletTransactionService(
    IWalletTransactionRepository walletTransactionRepository,
    IPlayerRepository playerRepository
    ) : IWalletTransactionService
{
    private WalletTransactionDb CreateTransaction(Guid playerId, Guid transactionId, WalletTransactionType type, WalletTransactionState state, decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than 0", nameof(amount));
        }
        
        return new WalletTransactionDb(playerId, transactionId, (byte)type, (byte)state, amount);
    }
    
    public IEnumerable<WalletTransactionResponse> GetTransactions(Guid playerId)
    {
        return walletTransactionRepository
            .GetTransactions(playerId)
            .Select(t => 
                new WalletTransactionResponse(
                    t.PlayerId, 
                    t.TransactionId, 
                    ((WalletTransactionType)t.TransactionType).ToString(), 
                    ((WalletTransactionState)t.TransactionState).ToString(), 
                    t.Amount
                    )
            );
    }

    public WalletTransactionState ExecuteTransaction(Player player, Guid transactionId, WalletTransactionType transactionType, decimal amount)
    {
        var transaction = walletTransactionRepository.GetTransaction(transactionId);
        
        if (transaction != null)
        {
            return (WalletTransactionState)transaction.TransactionState;
        }

        var state = WalletTransactionState.Pending;
        var balance = player.Balance!.Value;
        
        switch (transactionType)
        {
            case WalletTransactionType.Deposit:
                balance += amount;
                state = WalletTransactionState.Accepted;
                break;
            case WalletTransactionType.Stake:
                if (balance < amount)
                {
                    state = WalletTransactionState.Rejected;
                }
                else
                {
                    balance -= amount;
                    state = WalletTransactionState.Accepted;                    
                }
                break;
            case WalletTransactionType.Win:
                balance += amount;
                state = WalletTransactionState.Accepted;
                break;
            default:
                // Unexpected transaction type.
                return WalletTransactionState.Rejected;
        }

        if (state == WalletTransactionState.Accepted)
        {
            playerRepository.SetBalance(player.Id, balance);
        }

        transaction = CreateTransaction(player.Id, transactionId, transactionType, state, amount);
        walletTransactionRepository.SaveTransaction(transaction);
        return state;
    }
}