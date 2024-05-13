using System.ComponentModel.DataAnnotations;

namespace PlayerWallet.Model.Domain.Players.ApiModels;

public record struct TransactionRequest
{
    [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public required decimal Amount { get; init; }
}