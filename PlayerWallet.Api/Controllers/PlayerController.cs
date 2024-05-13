using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using PlayerWallet.Model.Domain.Players.ApiModels;
using PlayerWallet.Model.Domain.Players.Services;
using PlayerWallet.Model.Domain.Wallets;
using PlayerWallet.Model.Domain.Wallets.ApiModels;
using PlayerWallet.Model.Domain.Wallets.Services;

namespace PlayerWallet.Api.Controllers;

[ApiController]
[Route("api/players")]
public class PlayerController(IPlayerService playerService, IWalletTransactionService walletTransactionService) : ControllerBase
{
    #region Players

    [HttpPost("")]
    public ActionResult<PlayerResponse> CreatePlayer()
    {
        return Ok(playerService.CreatePlayer());
    }

    #endregion

    #region Wallet
    
    [HttpPut("{playerId:guid}/register-wallet")]
    public ActionResult RegisterWallet(Guid playerId)
    {
        playerService.RegisterWallet(playerId);
        return Ok();
    }
    
    [OutputCache(PolicyName = "PlayerOutputCache")]
    [HttpGet("{playerId:guid}/balance")]
    public ActionResult<PlayerResponse> GetBalance(Guid playerId)
    {
        return Ok(playerService.GetBalance(playerId));
    }
    
    [HttpGet("{playerId:guid}/transactions")]
    public ActionResult<WalletTransactionResponse> GetWalletTransactions(Guid playerId)
    {
        return Ok(walletTransactionService.GetTransactions(playerId));
    }
    
    [HttpPut("{playerId:guid}/transactions/{transactionId:guid}/deposit")]
    public ActionResult<TransactionResponse> Deposit(Guid playerId, Guid transactionId, TransactionRequest request)
    {
        return Ok(playerService.ExecuteWalletOperation(playerId, transactionId, WalletTransactionType.Deposit, request.Amount));
    }
    
    [HttpPut("{playerId:guid}/transactions/{transactionId:guid}/stake")]
    public ActionResult<TransactionResponse> Stake(Guid playerId, Guid transactionId, TransactionRequest request)
    {
        return Ok(playerService.ExecuteWalletOperation(playerId, transactionId, WalletTransactionType.Stake, request.Amount));
    }
    
    [HttpPut("{playerId:guid}/transactions/{transactionId:guid}/win")]
    public ActionResult<TransactionResponse> Win(Guid playerId, Guid transactionId, TransactionRequest request)
    {
        return Ok(playerService.ExecuteWalletOperation(playerId, transactionId, WalletTransactionType.Win, request.Amount));
    }
    
    #endregion
}