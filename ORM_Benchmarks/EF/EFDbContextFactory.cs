using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ORM_Benchmarks.EF;

public class EFDbContextFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IDbContextFactory<EFDbContext> _factory;

    public EFDbContextFactory(string connectionString)
    {
        var services = new ServiceCollection();
        services.AddPooledDbContextFactory<EFDbContext>(options =>
            options.UseSqlServer(connectionString), 100);

        _serviceProvider = services.BuildServiceProvider();
        _factory = _serviceProvider.GetRequiredService<IDbContextFactory<EFDbContext>>();
    }

    public EFDbContext CreateDbContext()
    {
        return _factory.CreateDbContext();
    }

    public void Dispose()
    {
        (_serviceProvider as IDisposable)?.Dispose();
    }
}
