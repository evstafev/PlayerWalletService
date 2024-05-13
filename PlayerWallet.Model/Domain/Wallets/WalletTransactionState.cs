namespace PlayerWallet.Model.Domain.Wallets;

public enum WalletTransactionState
{
    /// <summary>
    /// Transaction is in a pending state.
    /// </summary>
    Pending, 
    /// <summary>
    /// Transaction has been accepted.
    /// </summary>
    Accepted,
    /// <summary>
    /// Transaction has been rejected.
    /// </summary>
    Rejected
}