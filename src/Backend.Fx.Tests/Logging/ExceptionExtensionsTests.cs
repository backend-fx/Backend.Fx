using System;
using System.Linq;
using Backend.Fx.Logging;
using Xunit;

namespace Backend.Fx.Tests.Logging;

public class ExceptionExtensionsTests
{
    [Fact]
    public void GetInnerExceptionReturnsDeepestException()
    {
        var deepest = new InvalidOperationException("deepest");
        var middle = new Exception("middle", deepest);
        var outer = new Exception("outer", middle);

        Assert.Same(deepest, outer.GetInnerException());
    }

    [Fact]
    public void GetInnerExceptionReturnsSelfWhenNoInner()
    {
        var exception = new Exception("only");
        Assert.Same(exception, exception.GetInnerException());
    }

    [Fact]
    public void FromHierarchyYieldsWholeChain()
    {
        var deepest = new Exception("deepest");
        var outer = new Exception("outer", deepest);

        var chain = outer.FromHierarchy(ex => ex.InnerException!).ToList();

        Assert.Equal(2, chain.Count);
        Assert.Same(outer, chain[0]);
        Assert.Same(deepest, chain[1]);
    }

    [Fact]
    public void GetaAllMessagesReturnsEveryMessage()
    {
        var outer = new Exception("outer", new Exception("inner"));

        Assert.Equal(new[] { "outer", "inner" }, outer.GetaAllMessages().ToArray());
    }
}
