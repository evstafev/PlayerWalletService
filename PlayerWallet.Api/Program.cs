using PlayerWallet.Model.Caching;
using PlayerWallet.Model.Domain.Players.Services;
using PlayerWallet.Model.Domain.Wallets.Services;
using PlayerWallet.Model.Transactions;
using PlayerWallet.Repository.Players;
using PlayerWallet.Repository.Wallets;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IWalletTransactionService, WalletTransactionService>();
builder.Services.AddScoped<IWalletTransactionRepository, WalletTransactionRepository>();
builder.Services.AddScoped<ICacheService, CacheService>();

if (builder.Configuration.GetValue<bool>("IsSingleNode"))
{
    // For single-node setup in this project we use a Monitor to synchronize threads.
    builder.Services.AddScoped<ITransactionScopeFactory, TransactionScopeFactoryForSingleNode>();
}
else
{
    // For multi-node in real applications we can use sql transactions.
    builder.Services.AddScoped<ITransactionScopeFactory, TransactionScopeFactoryForMultiNode>();
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (!builder.Configuration.GetValue<bool>("IsSingleNode"))
{
    // For multi-node setup we need to use Redis for output caching.
    // For single-node setup we can use in-memory cache.
    builder.Services.AddStackExchangeRedisOutputCache(options =>
    {
        options.Configuration = builder.Configuration.GetConnectionString("RedisConnectionString");
        options.InstanceName = "RedisInstance";
    });
}

builder.Services.AddOutputCache(options =>
{
    // We use a custom policy to cache player's balance and mark each instance of cache with playerId tag.
    options.AddPolicy("PlayerOutputCache", build => build.AddPolicy<PlayerOutputCachePolicy>());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseOutputCache();
app.Run();
