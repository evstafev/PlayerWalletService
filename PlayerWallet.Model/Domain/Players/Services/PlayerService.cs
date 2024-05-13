using PlayerWallet.Model.Caching;
using PlayerWallet.Model.Domain.Players.ApiModels;
using PlayerWallet.Model.Domain.Wallets;
using PlayerWallet.Model.Domain.Wallets.Services;
using PlayerWallet.Model.Transactions;
using PlayerWallet.Repository.Players;

namespace PlayerWallet.Model.Domain.Players.Services;

public class PlayerService(
    IPlayerRepository playerRepository, 
    ITransactionScopeFactory transactionScopeFactory, 
    IWalletTransactionService walletTransactionService,
    ICacheService cacheService
    ) : IPlayerService
{
    #region Player

    public PlayerResponse CreatePlayer()
    {
        var id = Guid.NewGuid();
        var playerCreated = playerRepository.CreatePlayer(id);

        if (playerCreated)
        {
            return new PlayerResponse(id);
        }

        throw new Exception("Player was not created");
    }

    private Player GetPlayer(Guid playerId)
    {
        if (!playerRepository.PlayerExists(playerId))
        {
            throw new Exception($"Player with {playerId} was not found");
        }
        
        var balance = playerRepository.GetBalance(playerId);
        return new Player(playerId, balance);
    }
    
    private Player GetPlayerWithWalletOrThrow(Guid playerId)
    {
        var player = GetPlayer(playerId);
        
        if (!player.HasWallet)
        {
            throw new Exception($"Player with {playerId} has no wallet");
        }

        return player;
    }

    #endregion

    #region Wallet

    public void RegisterWallet(Guid playerId)
    {
        transactionScopeFactory.ExecuteAsUnitOfWork(playerId, () => RegisterWalletInternal(playerId));
    }
    
    private void RegisterWalletInternal(Guid playerId)
    {
        var player = GetPlayer(playerId);
        
        if (player.HasWallet)
        {
            throw new Exception($"Player with {playerId} has already a wallet");
        }
        
        playerRepository.SetBalance(playerId, 0);
    }

    public PlayerResponse GetBalance(Guid playerId)
    {
        var player = GetPlayerWithWalletOrThrow(playerId);
        return new PlayerResponse(playerId, player.Balance!.Value);
    }

    public TransactionResponse ExecuteWalletOperation(Guid playerId, Guid transactionId, WalletTransactionType transactionType, decimal amount)
    {
        var state = transactionScopeFactory.ExecuteAsUnitOfWork(playerId, () => ExecuteWalletOperationInternal(playerId, transactionId, transactionType, amount));
        return new TransactionResponse(playerId, transactionId, state.ToString());
    }
    
    private WalletTransactionState ExecuteWalletOperationInternal(Guid playerId, Guid transactionId, WalletTransactionType transactionType, decimal amount)
    {
        var player = GetPlayerWithWalletOrThrow(playerId);
        var state = walletTransactionService.ExecuteTransaction(player, transactionId, transactionType, amount);
            
        if (state == WalletTransactionState.Accepted)
        {
            cacheService.ClearOutputCache(player.Id.ToString());
        }

        return state;
    }
    
    #endregion
}