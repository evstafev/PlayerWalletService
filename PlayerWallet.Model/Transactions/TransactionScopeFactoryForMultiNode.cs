using System.Transactions;

namespace PlayerWallet.Model.Transactions;

public class TransactionScopeFactoryForMultiNode : ITransactionScopeFactory
{
    public void ExecuteAsUnitOfWork(Guid playerId, Action callback)
    {
        using var scope = MakeWriteScope();
        callback();
        scope.Complete();
    }
    
    public T ExecuteAsUnitOfWork<T>(Guid playerId, Func<T> callback)
    {
        using var scope = MakeWriteScope();
        var result = callback();
        scope.Complete();
        return result;
    }

    private TransactionScope MakeWriteScope()
    {
        return new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TransactionManager.MaximumTimeout
        });
    }
}