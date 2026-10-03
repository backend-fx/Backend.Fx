using System;
using System.Threading.Tasks;
using Backend.Fx.Util;
using Xunit;

namespace Backend.Fx.Tests.Util;

public class AsyncHelperTests
{
    [Fact]
    public void RunSyncExecutesVoidTask()
    {
        var executed = false;

        AsyncHelper.RunSync(async () =>
        {
            await Task.Delay(10);
            executed = true;
        });

        Assert.True(executed);
    }

    [Fact]
    public void RunSyncReturnsResultOfTask()
    {
        var result = AsyncHelper.RunSync(async () =>
        {
            await Task.Delay(10);
            return 42;
        });

        Assert.Equal(42, result);
    }

    [Fact]
    public void RunSyncPropagatesExceptionAsAggregateException()
    {
        var exception = Assert.Throws<AggregateException>(() =>
            AsyncHelper.RunSync(async () =>
            {
                await Task.Delay(10);
                throw new InvalidOperationException("boom");
            }));

        Assert.IsType<InvalidOperationException>(exception.InnerException);
        Assert.Equal("boom", exception.InnerException!.Message);
    }
}
