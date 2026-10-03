using Backend.Fx.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Backend.Fx.Tests.Logging;

public class LogTests
{
    [Fact]
    public void CreateOverloadsReturnNonNullLoggers()
    {
        Assert.NotNull(Log.Create<LogTests>());
        Assert.NotNull(Log.Create(typeof(LogTests)));
        Assert.NotNull(Log.Create("category"));
    }

    [Fact]
    public void InitAsyncLocalOverridesFactoryWithinScopeAndRestoresAfterwards()
    {
        var factory = new TestLoggerFactory();

        using (Log.InitAsyncLocal(factory))
        {
            Log.Create("scoped");
            Assert.Contains("scoped", factory.CreatedCategories);
        }

        factory.CreatedCategories.Clear();
        Log.Create("after-scope");
        Assert.DoesNotContain("after-scope", factory.CreatedCategories);
    }

    [Fact]
    public void InitializeSetsGlobalFactory()
    {
        var factory = new TestLoggerFactory();
        try
        {
            Log.Initialize(factory);
            Log.Create("global");
            Assert.Contains("global", factory.CreatedCategories);
        }
        finally
        {
            Log.Initialize(new NullLoggerFactory());
        }
    }
}
