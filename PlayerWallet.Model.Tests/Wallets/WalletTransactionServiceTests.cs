using Moq;
using PlayerWallet.Model.Domain.Players;
using PlayerWallet.Model.Domain.Wallets;
using PlayerWallet.Model.Domain.Wallets.Services;
using PlayerWallet.Repository.Players;
using PlayerWallet.Repository.Wallets;
using PlayerWallet.Repository.Wallets.DataModels;

namespace PlayerWallet.Model.Tests.Wallets;

[TestFixture]
public class WalletTransactionServiceTests
{
    private readonly Mock<IWalletTransactionRepository> _mockWalletTransactionRepository;
    private readonly Mock<IPlayerRepository> _mockPlayerRepository;
    private readonly WalletTransactionService _walletTransactionService;

    public WalletTransactionServiceTests()
    {
        _mockWalletTransactionRepository = new Mock<IWalletTransactionRepository>();
        _mockPlayerRepository = new Mock<IPlayerRepository>();
        _walletTransactionService = new WalletTransactionService(_mockWalletTransactionRepository.Object, _mockPlayerRepository.Object);
    }

    [TestCase(WalletTransactionState.Rejected)]
    [TestCase(WalletTransactionState.Accepted)]
    public void ExecuteTransaction_ReturnsTransactionState_WhenTransactionExists(WalletTransactionState state)
    {
        var playerId = Guid.NewGuid();
        var transactionId = Guid.NewGuid();
        var amount = 100;
        
        _mockWalletTransactionRepository
            .Setup(r => r.GetTransaction(It.IsAny<Guid>()))
            .Returns(new WalletTransactionDb(playerId, transactionId, (byte)It.IsAny<WalletTransactionType>(), (byte)state, amount));
        
        var player = new Player(playerId, amount);
        var result = _walletTransactionService.ExecuteTransaction(player, transactionId, It.IsAny<WalletTransactionType>(), amount);
        Assert.That(result, Is.EqualTo(state));
    }
    
    [Test]
    public void ExecuteTransaction_ReturnsAccepted_WhenDeposit()
    {
        var playerId = Guid.NewGuid();
        var transactionId = Guid.NewGuid();
        var amount = 100;
        var player = new Player(playerId, 0);

        var result = _walletTransactionService.ExecuteTransaction(player, transactionId, WalletTransactionType.Deposit, amount);
        Assert.That(result, Is.EqualTo(WalletTransactionState.Accepted));
    }

    [Test]
    public void ExecuteTransaction_ReturnsAccepted_WhenStakeAndBalanceIsEnough()
    {
        var playerId = Guid.NewGuid();
        var transactionId = Guid.NewGuid();
        var amount = 50;
        var player = new Player(playerId, 100);

        var result = _walletTransactionService.ExecuteTransaction(player, transactionId, WalletTransactionType.Stake, amount);
        Assert.That(result, Is.EqualTo(WalletTransactionState.Accepted));
    }

    [Test]
    public void ExecuteTransaction_ReturnsRejected_WhenStakeAndBalanceIsNotEnough()
    {
        var playerId = Guid.NewGuid();
        var transactionId = Guid.NewGuid();
        var amount = 150;
        var player = new Player(playerId, 100);

        var result = _walletTransactionService.ExecuteTransaction(player, transactionId, WalletTransactionType.Stake, amount);
        Assert.That(result, Is.EqualTo(WalletTransactionState.Rejected));
    }

    [Test]
    public void ExecuteTransaction_ReturnsAccepted_WhenWin()
    {
        var playerId = Guid.NewGuid();
        var transactionId = Guid.NewGuid();
        var amount = 100;
        var player = new Player(playerId, 0);

        var result = _walletTransactionService.ExecuteTransaction(player, transactionId, WalletTransactionType.Win, amount);
        Assert.That(result, Is.EqualTo(WalletTransactionState.Accepted));
    }

    [Test]
    public void ExecuteTransaction_ReturnsRejected_WhenUnexpectedTransactionType()
    {
        var playerId = Guid.NewGuid();
        var transactionId = Guid.NewGuid();
        var amount = 100;
        var player = new Player(playerId, 0);

        var result = _walletTransactionService.ExecuteTransaction(player, transactionId, (WalletTransactionType)100, amount);
        Assert.That(result, Is.EqualTo(WalletTransactionState.Rejected));
    }
}