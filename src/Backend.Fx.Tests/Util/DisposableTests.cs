using System;
using System.Collections.Generic;
using Backend.Fx.Util;
using Xunit;

namespace Backend.Fx.Tests.Util;

public class DelegateDisposableTests
{
    [Fact]
    public void InvokesActionOnDispose()
    {
        var disposed = false;
        var sut = new DelegateDisposable(() => disposed = true);

        Assert.False(disposed);
        sut.Dispose();
        Assert.True(disposed);
    }

    [Fact]
    public void ThrowsForNullAction()
        => Assert.Throws<ArgumentNullException>(() => new DelegateDisposable(null!));
}

public class MultipleDisposableTests
{
    [Fact]
    public void DisposesAllInOrder()
    {
        var order = new List<int>();
        var sut = new MultipleDisposable(
            new DelegateDisposable(() => order.Add(1)),
            new DelegateDisposable(() => order.Add(2)),
            new DelegateDisposable(() => order.Add(3)));

        sut.Dispose();

        Assert.Equal(new[] { 1, 2, 3 }, order);
    }

    [Fact]
    public void DisposeWithoutDisposablesDoesNotThrow()
    {
        var sut = new MultipleDisposable();
        sut.Dispose();
    }
}
