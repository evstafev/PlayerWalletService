using PlayerWallet.Model.Domain.Wallets;

namespace PlayerWallet.Model.Domain.Players.ApiModels;

public record TransactionResponse(Guid PlayerId, Guid TransactionId, string TransactionState);