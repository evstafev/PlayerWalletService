using System.Collections.Concurrent;

namespace PlayerWallet.Repository.Players;

public class PlayerRepository : IPlayerRepository
{
    private static readonly ConcurrentDictionary<Guid, decimal?> Players = new();

    public bool CreatePlayer(Guid playerId)
    {
        return Players.TryAdd(playerId, null);
    }

    public decimal? GetBalance(Guid playerId)
    {
        return Players[playerId];
    }

    public bool PlayerExists(Guid playerId)
    {
        return Players.ContainsKey(playerId);
    }

    public void SetBalance(Guid playerId, decimal balance)
    {
        Players[playerId] = balance;
    }
}