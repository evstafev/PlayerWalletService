using PlayerWallet.Model.Domain.Players;

namespace PlayerWallet.Model.Tests.Players;

[TestFixture]
public class PlayerTests
{
    [Test]
    public void Player_WithIdAndBalance_HasWallet()
    {
        var playerId = Guid.NewGuid();
        var balance = 100;
        var player = new Player(playerId, balance);
        Assert.True(player.HasWallet);
    }

    [Test]
    public void Player_WithIdAndNullBalance_HasNoWallet()
    {
        var playerId = Guid.NewGuid();
        var player = new Player(playerId, null);
        Assert.False(player.HasWallet);
    }

    [Test]
    public void Player_WithIdAndBalance_ReturnsCorrectIdAndBalance()
    {
        var playerId = Guid.NewGuid();
        var balance = 100;
        var player = new Player(playerId, balance);
        Assert.That(player.Id, Is.EqualTo(playerId));
        Assert.That(player.Balance, Is.EqualTo(balance));
    }
}