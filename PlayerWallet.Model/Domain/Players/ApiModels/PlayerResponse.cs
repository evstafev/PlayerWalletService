namespace PlayerWallet.Model.Domain.Players.ApiModels;

public record struct PlayerResponse(Guid Id, decimal? Balance = null);