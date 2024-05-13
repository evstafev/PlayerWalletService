using Moq;
using PlayerWallet.Model.Caching;
using PlayerWallet.Model.Domain.Players;
using PlayerWallet.Model.Domain.Players.Services;
using PlayerWallet.Model.Domain.Wallets;
using PlayerWallet.Model.Domain.Wallets.Services;
using PlayerWallet.Model.Transactions;
using PlayerWallet.Repository.Players;

namespace PlayerWallet.Model.Tests.Players;

[TestFixture]
public class PlayerServiceTests
{
    private readonly Mock<IPlayerRepository> _mockPlayerRepository;
    private readonly Mock<IWalletTransactionService> _mockWalletTransactionService;
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly PlayerService _playerService;

    public PlayerServiceTests()
    {
        _mockPlayerRepository = new Mock<IPlayerRepository>();
        _mockWalletTransactionService = new Mock<IWalletTransactionService>();
        _mockCacheService = new Mock<ICacheService>();
        _playerService = new PlayerService(
            _mockPlayerRepository.Object, new TransactionScopeFactoryForSingleNode(), _mockWalletTransactionService.Object, _mockCacheService.Object
            );
    }

    [Test]
    public void CreatePlayer_ReturnsPlayerResponse_WhenPlayerCreated()
    {
        _mockPlayerRepository.Setup(repo => repo.CreatePlayer(It.IsAny<Guid>())).Returns(true);

        Assert.DoesNotThrow(()  => _playerService.CreatePlayer());
    }

    [Test]
    public void CreatePlayer_ThrowsException_WhenPlayerNotCreated()
    {
        _mockPlayerRepository.Setup(repo => repo.CreatePlayer(It.IsAny<Guid>())).Returns(false);

        var exception = Assert.Throws<Exception>(() => _playerService.CreatePlayer());
        Assert.That(exception.Message, Is.EqualTo("Player was not created"));
    }

    [Test]
    public void RegisterWallet_ThrowsException_WhenNoPlayer()
    {
        var id = Guid.NewGuid();
        _mockPlayerRepository.Setup(repo => repo.PlayerExists(id)).Returns(false);

        var exception = Assert.Throws<Exception>(() => _playerService.RegisterWallet(id));
        Assert.That(exception.Message, Is.EqualTo($"Player with {id} was not found"));
    }
    
    [Test]
    public void RegisterWallet_ThrowsException_WhenPlayerHasWallet()
    {
        var id = Guid.NewGuid();
        _mockPlayerRepository.Setup(repo => repo.PlayerExists(id)).Returns(true);
        _mockPlayerRepository.Setup(repo => repo.GetBalance(id)).Returns(100);

        var exception = Assert.Throws<Exception>(() => _playerService.RegisterWallet(id));
        Assert.That(exception.Message, Is.EqualTo($"Player with {id} has already a wallet"));
    }
    
    [Test]
    public void RegisterWallet_DoesNotThrowException_WhenPlayerHasNoWallet()
    {
        var id = Guid.NewGuid();
        _mockPlayerRepository.Setup(repo => repo.PlayerExists(id)).Returns(true);
        _mockPlayerRepository.Setup(repo => repo.GetBalance(id)).Returns((decimal?)null);

        Assert.DoesNotThrow(() => _playerService.RegisterWallet(id));
    }

    [Test]
    public void GetBalance_ThrowsException_WhenNoPlayer()
    {
        var id = Guid.NewGuid();
        _mockPlayerRepository.Setup(repo => repo.PlayerExists(id)).Returns(false);

        var exception = Assert.Throws<Exception>(() => _playerService.GetBalance(id));
        Assert.That(exception.Message, Is.EqualTo($"Player with {id} was not found"));
    }
    
    [Test]
    public void GetBalance_ThrowsException_WhenPlayerHasNoWallet()
    {
        var id = Guid.NewGuid();
        _mockPlayerRepository.Setup(repo => repo.PlayerExists(id)).Returns(true);
        _mockPlayerRepository.Setup(repo => repo.GetBalance(id)).Returns((decimal?)null);

        var exception = Assert.Throws<Exception>(() => _playerService.GetBalance(id));
        Assert.That(exception.Message, Is.EqualTo($"Player with {id} has no wallet"));
    }
    
    [Test]
    public void GetBalance_ReturnsPlayerResponse_WhenPlayerHasWallet()
    {
        var id = Guid.NewGuid();
        _mockPlayerRepository.Setup(repo => repo.PlayerExists(id)).Returns(true);
        _mockPlayerRepository.Setup(repo => repo.GetBalance(id)).Returns(100);

        var result = _playerService.GetBalance(id);

        Assert.That(result.Id, Is.EqualTo(id));
        Assert.That(result.Balance, Is.EqualTo(100));
    }
    
    [Test]
    public void ExecuteWalletOperation_ThrowsException_WhenNoPlayer()
    {
        var playerId = Guid.NewGuid();
        var transactionId = Guid.NewGuid();
        var amount = 100;
        
        _mockPlayerRepository.Setup(repo => repo.PlayerExists(playerId)).Returns(false);

        var exception = Assert.Throws<Exception>(() => _playerService.ExecuteWalletOperation(playerId, transactionId, WalletTransactionType.Deposit, amount));
        Assert.That(exception.Message, Is.EqualTo($"Player with {playerId} was not found"));
    }
    
    [Test]
    public void ExecuteWalletOperation_ThrowsException_WhenPlayerHasNoWallet()
    {
        var playerId = Guid.NewGuid();
        var transactionId = Guid.NewGuid();
        var amount = 100;
        
        _mockPlayerRepository.Setup(repo => repo.PlayerExists(playerId)).Returns(true);

        var exception = Assert.Throws<Exception>(() => _playerService.ExecuteWalletOperation(playerId, transactionId, WalletTransactionType.Deposit, amount));
        Assert.That(exception.Message, Is.EqualTo($"Player with {playerId} has no wallet"));
    }

    [TestCase(WalletTransactionType.Win, WalletTransactionState.Rejected)]
    [TestCase(WalletTransactionType.Stake, WalletTransactionState.Rejected)]
    [TestCase(WalletTransactionType.Deposit, WalletTransactionState.Rejected)]
    [TestCase(WalletTransactionType.Win, WalletTransactionState.Accepted)]
    [TestCase(WalletTransactionType.Stake, WalletTransactionState.Accepted)]
    [TestCase(WalletTransactionType.Deposit, WalletTransactionState.Accepted)]
    public void ExecuteWalletOperation_ReturnsTransactionResponse_WhenTransactionAccepted(WalletTransactionType type, WalletTransactionState state)
    {
        var playerId = Guid.NewGuid();
        var transactionId = Guid.NewGuid();
        var amount = 100;
        
        _mockPlayerRepository.Setup(repo => repo.PlayerExists(playerId)).Returns(true);
        _mockPlayerRepository.Setup(repo => repo.GetBalance(playerId)).Returns(100);
        _mockWalletTransactionService.Setup(service => service
            .ExecuteTransaction(It.IsAny<Player>(), transactionId, It.IsAny<WalletTransactionType>(), amount)
            )
            .Returns(state);

        var result = _playerService.ExecuteWalletOperation(playerId, transactionId, type, amount);

        Assert.That(result.PlayerId, Is.EqualTo(playerId));
        Assert.That(result.TransactionId, Is.EqualTo(transactionId));
        Assert.That(result.TransactionState, Is.EqualTo(state.ToString()));
    }

    
}