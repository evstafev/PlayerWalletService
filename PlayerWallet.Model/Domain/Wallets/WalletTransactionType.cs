namespace PlayerWallet.Model.Domain.Wallets;

public enum WalletTransactionType
{
    /// <summary>
    /// Increments balance
    /// </summary>
    Deposit, 
    /// <summary>
    /// Decrements balance
    /// </summary>
    Stake, 
    /// <summary>
    /// Increments balance
    /// </summary>
    Win
}