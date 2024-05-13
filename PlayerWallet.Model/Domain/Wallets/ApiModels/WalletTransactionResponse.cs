namespace PlayerWallet.Model.Domain.Wallets.ApiModels;

public record WalletTransactionResponse(Guid PlayerId, Guid TransactionId, string TransactionType, string TransactionState, decimal Amount);