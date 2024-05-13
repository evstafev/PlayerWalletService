namespace PlayerWallet.Repository.Wallets.DataModels;

public record WalletTransactionDb(Guid PlayerId, Guid TransactionId, byte TransactionType, byte TransactionState, decimal Amount);
