using System.Collections.Concurrent;

namespace PlayerWallet.Model.Transactions;

public class TransactionScopeFactoryForSingleNode : ITransactionScopeFactory
{
    private static readonly ConcurrentDictionary<string, object> SyncObjects = new();
    
    public void ExecuteAsUnitOfWork(Guid playerId, Action callback)
    {
        lock (GetSyncObject(playerId.ToString()))
        {
            callback();
        }
    }
    
    public T ExecuteAsUnitOfWork<T>(Guid playerId, Func<T> callback)
    {
        lock (GetSyncObject(playerId.ToString()))
        {
            return callback();
        }
    }
    
    private static object GetSyncObject(string key)
    {
        return SyncObjects.GetOrAdd(key, new object());
    }
}